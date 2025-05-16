using Data;
using System.Collections.Generic;
using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using CustomUtility;
using Zenject;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using Unity.Jobs;
namespace GamePlay
{
    /// <summary>
    /// Enemy를 생성, 제거를 관리하는 클레스
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class WaveSystem : MonoBehaviour
    {
        [Inject] private DataManager _dataManager;
        [Inject] private GameDataHub _gameDataHub;
        public StageType CurStageType {  get; private set; }
        private IWaveStrategy _waveStrategy; // 어떤 Wave를 발생 시킬지 정하는 전략 
        private float3 _spawnPosition; // 생성 위치
        private SpawnData _preSpawnData; // 이전 생성 데이터
        private SpawnData _spawnData; // 생성데이터
        private GameObject _enemyPrefab; // 어떤 Enemy를 생성할지 runtime 중에 Addressable로 불러옴
        private ObjectPool<ObjectPoolItem> _objectPool; // EnemyObjectPool 


        public void SetSpawnPosition(float3 spawnPosition) {  _spawnPosition = spawnPosition; }
        /// <summary>
        /// Stage가 시작 될때 호출되는 함수
        /// </summary>
        public void SpawnEnemiesWave(StageType type, int stageLevel) {
            SetWaveStrategy(type); // 웨이브 전략을 type에 맞춰 정함

            // Spawn Data 생성
            _preSpawnData = _spawnData;
            _spawnData = _waveStrategy.GetSpawnData(stageLevel, _spawnPosition);
            
            EnemyData[] enemyDatas = new EnemyData[_spawnData.spawnCount];
            for(int i = 0; i < _spawnData.spawnCount; i++) {
                enemyDatas[i] =_spawnData.enemyData;
            }

            // NativeArray 생성
            
            var spawnEnemyDatas = new NativeArray<EnemyData>(enemyDatas, Allocator.Persistent);
            _gameDataHub.SetEnemiesData(spawnEnemyDatas); // 허브에 등록

            // Enemy Prefab 로드
            if (_preSpawnData != null && (_preSpawnData.spawnEnemyKey == _spawnData.spawnEnemyKey)) {
                _dataManager.ReleaseAsset(_preSpawnData.spawnEnemyKey); // 메모리 해제
            } else { // 이전 데이터가 없으면 생성
                if(_objectPool != null) _objectPool.Dipose();
                _dataManager.LoadAssetAsync<GameObject>(_spawnData.spawnEnemyKey).ContinueWith((p) => {
                    _enemyPrefab = p;
                    _objectPool = ObjectPoolBuilder<ObjectPoolItem>.Instance(_enemyPrefab).Build(); // pool 생성
                });
            }
          

        }
        private void SetWaveStrategy(StageType type) {
            switch (type) {
                case StageType.Standard:
                _waveStrategy = new StandardWaveStrategy();
                break;
                case StageType.Boss:
                _waveStrategy = new BossWaveStrategy();
                break;
                case StageType.Timer:
                _waveStrategy = new TimerWaveStrategy();
                break;
            }
        }

        private void Update() {
            NativeArray<EnemyData> enemiesData = _gameDataHub.GetEnemiesData();
            List<ObjectPoolItem> enemyPoolItemList = _gameDataHub.enemyPoolItemList;
            if (_spawnData == null || enemiesData.Length == 0 || _objectPool == null) return;

            // 시간 지남에 따른 spawn
            if(_spawnData.curRemainingTime <= 0 && _spawnData.curSpawnIndex < _spawnData.spawnCount) {
                _spawnData.curRemainingTime += _spawnData.spawnInterval;
                var enemyData = enemiesData[_spawnData.curSpawnIndex];
                enemyData.isSpawn = true;
                enemiesData[_spawnData.curSpawnIndex] = enemyData;
                _spawnData.curSpawnIndex++;

                // 생성
                var item = _objectPool.BorrowItem();
                item.gameObject.SetActive(true);

                // 할당
                enemyPoolItemList.Add(item);
            }

            // ObjectPool 회수
            for (int i = 0; i < enemiesData.Length; i++) {
                EnemyData enemyData = enemiesData[i];
                if (!enemyData.isSpawn) return;
                if (enemyData.isDead && enemyData.isObj) {
                    // 회수
                    enemyData.isObj = false;
                    enemyPoolItemList[i].gameObject.SetActive(false);
                    _objectPool.RepayItem(enemyPoolItemList[i]);
                    enemiesData[i] = enemyData;
                }
            }
            _spawnData.curRemainingTime -= Time.deltaTime;
        }

        
    }
}
