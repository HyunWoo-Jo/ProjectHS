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
    /// Enemy�� �����ϴ� Ŭ����
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class WaveSystem : MonoBehaviour
    {
        private EnemySystem _enemySystem;
        [Inject] private DataManager _dataManager;
        public StageType CurStageType {  get; private set; }
        private IWaveStrategy _waveStrategy; // � Wave�� �߻� ��ų�� ���ϴ� ���� 
        private float3 _spawnPosition; // ���� ��ġ
        private SpawnData _preSpawnData; // ���� ���� ������
        private SpawnData _spawnData; // ����������
        private GameObject _enemyPrefab;
        private NativeArray<EnemyData> _spawnEnemyDatas; // ������ NativeArray
        private ObjectPool<ObjectPoolItem> _objectPool;


        public void SetSpawnPosition(float3 spawnPosition) {  _spawnPosition = spawnPosition; }
        /// <summary>
        /// Stage�� ���� �ɶ� ȣ��Ǵ� �Լ�
        /// </summary>
        public void SpawnEnemiesWave(StageType type, int stageLevel) {
            // �޸� ����
            SetWaveStrategy(type); // ���̺� ������ type�� ���� ����

            // Spawn Data ����
            _preSpawnData = _spawnData;
            _spawnData = _waveStrategy.GetSpawnData(stageLevel, _spawnPosition);
            
            EnemyData[] enemyDatas = new EnemyData[_spawnData.spawnCount];
            for(int i = 0; i < _spawnData.spawnCount; i++) {
                enemyDatas[i] =_spawnData.enemyData;
            }

            // NativeArray ����
            if (_spawnEnemyDatas.IsCreated) { _spawnEnemyDatas.Dispose(); }
            _spawnEnemyDatas = new NativeArray<EnemyData>(enemyDatas, Allocator.Persistent);

            // Enemy Prefab �ε�
            if(_preSpawnData != null && (_preSpawnData.spawnEnemyKey == _spawnData.spawnEnemyKey)) {
                _dataManager.ReleaseAsset(_preSpawnData.spawnEnemyKey); // �޸� ����
            } else { // ���� �����Ͱ� ������ ����
                if(_objectPool != null) _objectPool.Dipose();
                _dataManager.LoadAssetAsync<GameObject>(_spawnData.spawnEnemyKey).ContinueWith((p) => {
                    _enemyPrefab = p;
                    _objectPool = ObjectPoolBuilder<ObjectPoolItem>.Instance(_enemyPrefab).Build(); // pool ����
                });
            }
            
            


            // EnemySystem�� Set
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

            // �ð� ������ ���� spawn
            if(_spawnData.curRemainingTime <= 0 && _spawnData.curSpawnIndex < _spawnData.spawnCount) {
                _spawnData.curRemainingTime += _spawnData.spawnInterval;
                var enemyData = _spawnEnemyDatas[_spawnData.curSpawnIndex];
                enemyData.isSpawn = true;
                _spawnEnemyDatas[_spawnData.curSpawnIndex] = enemyData;
                _spawnData.curSpawnIndex++;

                // ����
                var item = _objectPool.BorrowItem();
                item.gameObject.SetActive(true);

                // �Ҵ�
                _enemySystem.enemyObjectPoolItemList.Add(item);
            }

            // ObjectPool ȸ��
            for (int i = 0; i < _spawnEnemyDatas.Length; i++) {
                EnemyData enemyData = _spawnEnemyDatas[i];
                if (!enemyData.isSpawn) return;
                if (enemyData.isDead && enemyData.isObj) {
                    // ȸ��
                    _objectPool.RepayItem(_enemySystem.enemyObjectPoolItemList[i]);
                }
            }
            _spawnData.curRemainingTime -= Time.deltaTime;


        }

    }
}
