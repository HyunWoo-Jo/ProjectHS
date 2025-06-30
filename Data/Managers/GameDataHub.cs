using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;
using CustomUtility;
using Unity.Mathematics;
using System.Linq;
using System;
namespace Data
{
    public class GameDataHub : IEnemyDataService, IPositionService {
        private List<ObjectPoolItem> _enemyPoolItemList = new();

        // Data
        private NativeArray<EnemyData> _enemiesData;
        private NativeArray<float3> _paths; 
        private NativeArray<float3> _worldPosition;
        private int2 _mapSize;

        private List<SlotData> _slotDataList = new(); // Ÿ�� ����
        private List<TowerData> _towerDataList = new(); // ��ü Ÿ�� ���

        public List<ObjectPoolItem> GetEnemyPoolList() => _enemyPoolItemList;


        public void SetEnemiesData(NativeArray<EnemyData> data) {
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _enemiesData = data;
        }

        // grid �� ������ index�� ��ȯ // ���н� -1
        public int GetIndex(int x, int y) {
            // �Է� ���� �˻�
            if (x < 0 || x >= _mapSize.x)
                return -1;
            if (y < 0 || y >= _mapSize.y)
                return -1;
            // �ε��� ���
            return y * _mapSize.x + x;
        }
        public void SetMapSize(int x, int y) {
            _mapSize = new int2 { x = x, y = y };
        }
        public int2 GetMapSize() => _mapSize;

        /// <summary>
        /// enemiesData�� ����, ObjectPool�� ����
        /// </summary>
        /// <returns> ���� ������ ���� �ε��� </returns>
        public int MergeAliveEnemiesAndAppend(NativeArray<EnemyData> newWave) {
            // ���� �����Ͱ� ���ٸ� �ٷ� ��ü
            if (!_enemiesData.IsCreated) {
                _enemiesData = new NativeArray<EnemyData>(newWave, Allocator.Persistent);
                return 0;
            }

            // ��� �ִ� ���� ���� �ӽ� List�� ���͸�
            var aliveList = new List<EnemyData>(_enemiesData.Length);
            var alivePools = new List<ObjectPoolItem>(_enemiesData.Length);
            for (int i = 0; i < _enemiesData.Length; i++) {
                if (!_enemiesData[i].isDead) {
                    aliveList.Add(_enemiesData[i]);
                    alivePools.Add(_enemyPoolItemList[i]);     // pool�� ���� �ε����� ����

                }
            }
            int startIndex = aliveList.Count;

            // �� Wave ������ �ڿ� ���̱�
            aliveList.AddRange(newWave);          

            // NativeArray �Ҵ�
            var mergedArray = new NativeArray<EnemyData>(aliveList.ToArray(), Allocator.Persistent);

            // 5) ���� �迭 Dispose & �ʵ� ����
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _enemiesData = mergedArray;
            _enemyPoolItemList = alivePools;
            return startIndex;
        }
        public NativeArray<EnemyData> GetEnemiesData() => _enemiesData;

        public void SetTowerData(List<TowerData> data) {
            _towerDataList = data;
        }

        public List<TowerData> GetTowerData() => _towerDataList;

        public void SetPath(IEnumerable<Vector3> path) {
            if(_paths.IsCreated) _paths.Dispose();
            _paths = new NativeArray<float3>(path.Select(s => new float3(s.x, s.y, s.z)).ToArray(), Allocator.Persistent);
        }

        public NativeArray<float3> GetPath() => _paths;

        public int EnemiesLength() => _enemiesData.Length;

        public EnemyData GetEnemyData(int index) {
            if(_enemiesData.Length <= index || -1 >= index) {
                return new EnemyData { // ���� ������ ��ȯ
                    isDead = true
                };   
            }
            return _enemiesData[index];
        }

        public void SetEnemyData(int index, EnemyData enemyData) {
            _enemiesData[index] = enemyData;
        }

        public float3 GetIndexToWorldPosition(int index) => _worldPosition[index];
        public void SetWorldPositionData(NativeArray<float3> arrays) {
            if (_worldPosition.IsCreated) _worldPosition.Dispose();
            _worldPosition = arrays;
        }

        public bool IsEnemyData() => _enemiesData.IsCreated;

        public void ClearSlotDataList() {
            _slotDataList.Clear();
        }
        public void AddSlot(SlotData slotData) {
            _slotDataList.Add(slotData);
        }
        public List<SlotData> GetSlotList() => _slotDataList;

        public SlotData GetSlotData(int index) => _slotDataList[index];

        ~GameDataHub() { // �Ҹ�
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _towerDataList = null;
            if (_paths.IsCreated) _paths.Dispose();
            if(_worldPosition.IsCreated) _worldPosition.Dispose();
        }
    }   
}
