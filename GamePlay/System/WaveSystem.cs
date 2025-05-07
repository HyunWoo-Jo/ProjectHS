using Data;
using System.Collections.Generic;
using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using CustomUtility;
using Zenject;
using Cysharp.Threading.Tasks;
namespace GamePlay
{
    /// <summary>
    /// Enemy를 생성하는 클레스
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class WaveSystem : MonoBehaviour
    {
        private EnemySystem _enemySystem;
        [Inject] private DataManager _dataManager;
        public StageType CurStageType {  get; private set; }
        private IWaveStrategy _waveStrategy; // 어떤 Wave를 발생 시킬지 정하는 전략 
        private float3 _spawnPosition; // 생성 위치
        private SpawnData _preSpawnData; // 이전 생성 데이터
        private SpawnData _spawnData; // 생성데이터
        private GameObject _enemyPrefab;
        private NativeArray<EnemyData> _spawnEnemyDatas; // 생성된 NativeArray
        private ObjectPool<ObjectPoolItem> _objectPool;


        public void SetSpawnPosition(float3 spawnPosition) {  _spawnPosition = spawnPosition; }
        /// <summary>
        /// Stage가 시작 될때 호출되는 함수
        /// </summary>
        public void SpawnEnemiesWave(StageType type, int stageLevel) {
            // 메모리 해제
            SetWaveStrategy(type); // 웨이브 전략을 type에 맞춰 정함

            // Spawn Data 생성
            _preSpawnData = _spawnData;
            _spawnData = _waveStrategy.GetSpawnData(stageLevel, _spawnPosition);
            
            EnemyData[] enemyDatas = new EnemyData[_spawnData.spawnCount];
            for(int i = 0; i < _spawnData.spawnCount; i++) {
                enemyDatas[i] =_spawnData.enemyData;
            }

            // NativeArray 생성
            if (_spawnEnemyDatas.IsCreated) { _spawnEnemyDatas.Dispose(); }
            _spawnEnemyDatas = new NativeArray<EnemyData>(enemyDatas, Allocator.Persistent);

            // Enemy Prefab 로드
            if(_preSpawnData != null && (_preSpawnData.spawnEnemyKey == _spawnData.spawnEnemyKey)) {
                _dataManager.ReleaseAsset(_preSpawnData.spawnEnemyKey); // 메모리 해제
            } else { // 이전 데이터가 없으면 생성
                if(_objectPool != null) _objectPool.Dipose();
                _dataManager.LoadAssetAsync<GameObject>(_spawnData.spawnEnemyKey).ContinueWith((p) => {
                    _enemyPrefab = p;
                    _objectPool = ObjectPoolBuilder<ObjectPoolItem>.Instance(_enemyPrefab).Build(); // pool 생성
                });
            }
            
            


            // EnemySystem에 Set
            if (_enemySystem == null) {
                _enemySystem = GetComponent<EnemySystem>();
            }
            _enemySystem.SetEnemy(_spawnEnemyDatas);


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
            if (_spawnData == null || _spawnEnemyDatas.Length <= 0 || _objectPool == null) return;

            // 시간 지남에 따른 spawn
            if(_spawnData.curRemainingTime <= 0 && _spawnData.curSpawnIndex < _spawnData.spawnCount) {
                _spawnData.curRemainingTime += _spawnData.spawnInterval;
                var enemyData = _spawnEnemyDatas[_spawnData.curSpawnIndex];
                enemyData.isSpawn = true;
                _spawnEnemyDatas[_spawnData.curSpawnIndex] = enemyData;
                _spawnData.curSpawnIndex++;

                // 생성
                var item = _objectPool.BorrowItem();
                item.gameObject.SetActive(true);

                // 할당
                _enemySystem.enemyObjectPoolItemList.Add(item);
            }

            // ObjectPool 회수
            for (int i = 0; i < _spawnEnemyDatas.Length; i++) {
                EnemyData enemyData = _spawnEnemyDatas[i];
                if (!enemyData.isSpawn) return;
                if (enemyData.isDead && enemyData.isObj) {
                    // 회수
                    _objectPool.RepayItem(_enemySystem.enemyObjectPoolItemList[i]);
                }
            }
            _spawnData.curRemainingTime -= Time.deltaTime;


        }

    }
}
