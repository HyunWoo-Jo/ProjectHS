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
using UnityEngine.Assertions;
namespace GamePlay
{
    /// <summary>
    /// Enemy�� ����, ���Ÿ� �����ϴ� Ŭ����
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class WaveSystem : MonoBehaviour
    {
        [Inject] private DataManager _dataManager;
        [Inject] private GameDataHub _gameDataHub;
        [Inject] private StageSettingsModel _stageSettingsModel;
        public StageType CurStageType {  get; private set; }
        private IWaveStrategy _waveStrategy; // � Wave�� �߻� ��ų�� ���ϴ� ���� 
        private float3 _spawnPosition; // ���� ��ġ
        private SpawnData _spawnData; // ����������

        [Inject] private GameObjectPoolManager _gameObjectPoolManager;

        public void SetSpawnPosition(float3 spawnPosition) {  _spawnPosition = spawnPosition; }
        /// <summary>
        /// Stage�� ���� �ɶ� ȣ��Ǵ� �Լ�
        /// </summary>
        public void SpawnEnemiesWave(StageType type, int stageLevel) {
            SetWaveStrategy(type); // ���̺� ������ type�� ���� ����

            // Spawn Data ����
            _spawnData = _waveStrategy.GetSpawnData(stageLevel, _spawnPosition, _stageSettingsModel.stageDelayTime - 10);
            
            // Spawn Data�� ���� Enemy Data ����
            EnemyData[] enemyDatas = new EnemyData[_spawnData.spawnCount];
            for(int i = 0; i < _spawnData.spawnCount; i++) {
                enemyDatas[i] =_spawnData.enemyData;
            }

            // NativeArray ����
            var spawnEnemyDatas = new NativeArray<EnemyData>(enemyDatas, Allocator.Persistent);
            _spawnData.startIndex = _gameDataHub.MergeAliveEnemiesAndAppend(spawnEnemyDatas); // ��꿡 ��� , ���� �����Ͱ� ������ merge

            // Enemy Pool ���
            _gameObjectPoolManager.RegisterPool<ObjectPoolItem>(_spawnData.spawnEnemyPoolType); // ��� (�̹� �����ϸ� �ڵ� ��ŵ)
        }
        private void SetWaveStrategy(StageType type) {
            switch (type) {
                case StageType.Standard:
                _waveStrategy = new StandardWaveStrategy();
                break;
                case StageType.Boss:
                _waveStrategy = new BossWaveStrategy();
                break;
            }
        }

        private void Update() {
            if (GameSettings.IsPause) return;
            NativeArray<EnemyData> enemiesData = _gameDataHub.GetEnemiesData();
            List<ObjectPoolItem> enemyPoolItemList = _gameDataHub.GetEnemyPoolList();
            if (_spawnData == null || enemiesData.Length == 0) return;
            _spawnData.curRemainingTime -= Time.deltaTime;
            // �ð� ������ ���� spawn
            if (_spawnData.curRemainingTime <= 0 && _spawnData.curSpawnIndex < _spawnData.spawnCount) {
                _spawnData.curRemainingTime += _spawnData.spawnInterval;
                int index = _spawnData.curSpawnIndex + _spawnData.startIndex;
                var enemyData = enemiesData[index];
                enemyData.isSpawn = true;
                
                _spawnData.curSpawnIndex++;

                // enemy object ����
                var item = _gameObjectPoolManager.BorrowItem<ObjectPoolItem>(_spawnData.spawnEnemyPoolType);
                enemyData.isObj = true;

                // �Ҵ�
                enemyPoolItemList.Add(item);
                enemiesData[index] = enemyData;

            }


            // ������ ����
            // ObjectPool ȸ��
            for (int i = 0; i < enemiesData.Length; i++) {
                EnemyData enemyData = enemiesData[i];
                if (!enemyData.isSpawn) return; 
                if (enemyData.isDead && enemyData.isObj) {
                    enemyData.isObj = false;
                    // Object ȸ��
                    _gameObjectPoolManager.Repay(_spawnData.spawnEnemyPoolType, enemyPoolItemList[i]);
                    enemiesData[i] = enemyData;
                }
            }     
        }

        
    }
}
