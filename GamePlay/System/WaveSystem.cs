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
    /// Enemy�� ����, ���Ÿ� �����ϴ� Ŭ����
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class WaveSystem : MonoBehaviour
    {
        [Inject] private DataManager _dataManager;
        [Inject] private GameDataHub _gameDataHub;
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
            _spawnData = _waveStrategy.GetSpawnData(stageLevel, _spawnPosition);
            
            // Spawn Data�� ���� Enemy Data ����
            EnemyData[] enemyDatas = new EnemyData[_spawnData.spawnCount];
            for(int i = 0; i < _spawnData.spawnCount; i++) {
                enemyDatas[i] =_spawnData.enemyData;
            }

            // NativeArray ����
            var spawnEnemyDatas = new NativeArray<EnemyData>(enemyDatas, Allocator.Persistent);
            _gameDataHub.SetEnemiesData(spawnEnemyDatas); // ��꿡 ���

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
            // �ð� ������ ���� spawn
            if (_spawnData.curRemainingTime <= 0 && _spawnData.curSpawnIndex < _spawnData.spawnCount) {
                _spawnData.curRemainingTime += _spawnData.spawnInterval;
                var enemyData = enemiesData[_spawnData.curSpawnIndex];
                enemyData.isSpawn = true;
                enemiesData[_spawnData.curSpawnIndex] = enemyData;
                _spawnData.curSpawnIndex++;

                // ����
                var item = _gameObjectPoolManager.BorrowItem<ObjectPoolItem>(_spawnData.spawnEnemyPoolType);
                item.gameObject.SetActive(true);

                // �Ҵ�
                enemyPoolItemList.Add(item);
            }


            // ������ ����
            // ObjectPool ȸ��
            for (int i = 0; i < enemiesData.Length; i++) {
                EnemyData enemyData = enemiesData[i];
                if (!enemyData.isSpawn) return; 
                if (enemyData.isDead && enemyData.isObj) {
                    // ȸ��
                    enemyData.isObj = false;
                    enemyPoolItemList[i].gameObject.SetActive(false);
                    _gameObjectPoolManager.Repay(_spawnData.spawnEnemyPoolType, enemyPoolItemList[i]);
                    enemiesData[i] = enemyData;
                }
            }     
        }

        
    }
}
