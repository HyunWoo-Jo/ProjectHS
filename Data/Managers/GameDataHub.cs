using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;
using CustomUtility;
using Unity.Mathematics;
using System.Linq;
namespace Data
{
    public class GameDataHub : IEnemyDataService, IPositionService {
        private List<ObjectPoolItem> _enemyPoolItemList = new();

        private NativeArray<EnemyData> _enemiesData;
        private NativeArray<float3> _paths;
        private NativeArray<float3> _worldPosition;

        private List<SlotData> _slotDataList = new();
        private List<TowerData> _towerDataList = new();
        
        public List<ObjectPoolItem> GetEnemyPoolList() {
            return _enemyPoolItemList;
        }
        
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
                    alivePools.Add(_enemyPoolItemList[i]);     // pool도 같은 인덱스로 보존

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
            _enemyPoolItemList = alivePools;
            return startIndex;
        }
        public NativeArray<EnemyData> GetEnemiesData() => _enemiesData;

        public void SetTowerData(List<TowerData> data) {
            _towerDataList = data;
        }

        public List<TowerData> GetTowerData() {
            return _towerDataList;
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
            if(_enemiesData.Length <= index || -1 >= index) {
                return new EnemyData { // 죽은 데이터 반환
                    isDead = true
                };   
            }
            return _enemiesData[index];
        }

        public void SetEnemyData(int index, EnemyData enemyData) {
            _enemiesData[index] = enemyData;
        }

        public float3 GetGridToWorldPosition(int index) {
            return _worldPosition[index];
        }
        public void SetWorldPositionData(NativeArray<float3> arrays) {
            if (_worldPosition.IsCreated) _worldPosition.Dispose();
            _worldPosition = arrays;
        }

        public bool IsEnemyData() {
            return _enemiesData.IsCreated;
        }


        public void ClearSlotDataList() {
            _slotDataList.Clear();
        }
        public void AddSlot(SlotData slotData) {
            _slotDataList.Add(slotData);
        }
        public List<SlotData> GetSlotList() {
            return _slotDataList;
        }

        ~GameDataHub() { // 소멸
            if (_enemiesData.IsCreated) _enemiesData.Dispose();
            _towerDataList = null;
            if (_paths.IsCreated) _paths.Dispose();
            if(_worldPosition.IsCreated) _worldPosition.Dispose();
        }
    }   
}
