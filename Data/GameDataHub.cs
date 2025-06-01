using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;
using CustomUtility;
using Unity.Mathematics;
using System.Linq;
namespace Data
{
    public class GameDataHub : IEnemyDataService, IPositionService {
        public List<ObjectPoolItem> enemyPoolItemList = new();

        private NativeArray<EnemyData> _enemiesData;
        private List<TowerData> _towerData = new();
        private NativeArray<float3> _paths;
        private NativeArray<float3> _worldPosition;
        public void SetEnemiesData(NativeArray<EnemyData> data) {
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _enemiesData = data;
        }

        public NativeArray<EnemyData> GetEnemiesData() => _enemiesData;

        public void SetTowerData(List<TowerData> data) {
            _towerData = data;
        }

        public List<TowerData> GetTowerData() {
            return _towerData;
        }

        public void SetPath(IEnumerable<Vector3> path) {
            if(_paths.IsCreated) _paths.Dispose();
            _paths = new NativeArray<float3>(path.Select(s => new float3(s.x, s.y, s.z)).ToArray(), Allocator.Persistent);
        }

        public NativeArray<float3> GetPath() {
            return _paths;
        }

        public int EnemiesLength() {
            return _enemiesData.Length;
        }

        public EnemyData GetEnemyData(int index) {
            return _enemiesData[index];
        }

        public void SetEnemyData(int index, EnemyData enemyData) {
            _enemiesData[index] = enemyData;
        }

        public float3 GetGridToWorldPosition(int index) {
            return _worldPosition[index];
        }

        public bool IsEnemyData() {
            return _enemiesData.IsCreated;
        }

        ~GameDataHub() { // ¼Ò¸ê
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _towerData = null;
            if (_paths.IsCreated) _paths.Dispose();
        }
    }   
}
