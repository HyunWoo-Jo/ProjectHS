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

        /// <summary>
        /// enemiesData를 병합, ObjectPool을 유지
        /// </summary>
        /// <returns> 새로 생성된 시작 인덱스 </returns>
        public int MergeAliveEnemiesAndAppend(NativeArray<EnemyData> newWave) {
            // 기존 데이터가 없다면 바로 교체
            if (!_enemiesData.IsCreated) {
                _enemiesData = new NativeArray<EnemyData>(newWave, Allocator.Persistent);
                return 0;
            }

            // 살아 있는 기존 적만 임시 List로 필터링
            var aliveList = new List<EnemyData>(_enemiesData.Length);
            var alivePools = new List<ObjectPoolItem>(_enemiesData.Length);
            for (int i = 0; i < _enemiesData.Length; i++) {
                if (!_enemiesData[i].isDead) {
                    aliveList.Add(_enemiesData[i]);
                    if (!_enemiesData[i].isObj) {
                        alivePools.Add(enemyPoolItemList[i]);     // pool도 같은 인덱스로 보존
                    }
                   
                }
            }
            int startIndex = aliveList.Count;

            // 새 Wave 데이터 뒤에 붙이기
            aliveList.AddRange(newWave);          

            // NativeArray 할당
            var mergedArray = new NativeArray<EnemyData>(aliveList.ToArray(), Allocator.Persistent);

            // 5) 기존 배열 Dispose & 필드 갱신
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _enemiesData = mergedArray;
            enemyPoolItemList = alivePools;
            return startIndex;
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

        ~GameDataHub() { // 소멸
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _towerData = null;
            if (_paths.IsCreated) _paths.Dispose();
        }
    }   
}
