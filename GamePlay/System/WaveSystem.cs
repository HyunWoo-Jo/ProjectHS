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
        private SpawnData _spawnData; // 생성데이터

        [Inject] private GameObjectPoolManager _gameObjectPoolManager;


        public void SetSpawnPosition(float3 spawnPosition) {  _spawnPosition = spawnPosition; }
        /// <summary>
        /// Stage가 시작 될때 호출되는 함수
        /// </summary>
        public void SpawnEnemiesWave(StageType type, int stageLevel) {
            SetWaveStrategy(type); // 웨이브 전략을 type에 맞춰 정함

            // Spawn Data 생성
            _spawnData = _waveStrategy.GetSpawnData(stageLevel, _spawnPosition);
            
            // Spawn Data에 맞춰 Enemy Data 생성
            EnemyData[] enemyDatas = new EnemyData[_spawnData.spawnCount];
            for(int i = 0; i < _spawnData.spawnCount; i++) {
                enemyDatas[i] =_spawnData.enemyData;
            }

            // NativeArray 생성
            var spawnEnemyDatas = new NativeArray<EnemyData>(enemyDatas, Allocator.Persistent);
            _gameDataHub.SetEnemiesData(spawnEnemyDatas); // 허브에 등록

            // Enemy Pool 등록
            _gameObjectPoolManager.RegisterPool<ObjectPoolItem>(_spawnData.spawnEnemyPoolType); // 등록 (이미 존재하면 자동 스킵)
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
            if (_spawnData == null || enemiesData.Length == 0 ) return;
            _spawnData.curRemainingTime -= Time.deltaTime;
            // 시간 지남에 따른 spawn
            if (_spawnData.curRemainingTime <= 0 && _spawnData.curSpawnIndex < _spawnData.spawnCount) {
                _spawnData.curRemainingTime += _spawnData.spawnInterval;
                var enemyData = enemiesData[_spawnData.curSpawnIndex];
                enemyData.isSpawn = true;
                enemiesData[_spawnData.curSpawnIndex] = enemyData;
                _spawnData.curSpawnIndex++;

                // 생성
                var item = _gameObjectPoolManager.BorrowItem<ObjectPoolItem>(_spawnData.spawnEnemyPoolType);
                item.gameObject.SetActive(true);

                // 할당
                enemyPoolItemList.Add(item);
            }


            // 마지막 실행
            // ObjectPool 회수
            for (int i = 0; i < enemiesData.Length; i++) {
                EnemyData enemyData = enemiesData[i];
                if (!enemyData.isSpawn) return; 
                if (enemyData.isDead && enemyData.isObj) {
                    // 회수
                    enemyData.isObj = false;
                    enemyPoolItemList[i].gameObject.SetActive(false);
                    _gameObjectPoolManager.Repay(_spawnData.spawnEnemyPoolType, enemyPoolItemList[i]);
                    enemiesData[i] = enemyData;
                }
            }     
        }

        
    }
}
