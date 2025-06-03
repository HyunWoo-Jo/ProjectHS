using UnityEngine;
using System.Collections.Generic;
using CustomUtility;
using Zenject;
namespace Data
{


    /// <summary>
    /// ObjectPool���� �����ϴ� Manager
    /// </summary>
    public class GameObjectPoolManager : MonoBehaviour
    {
        [Inject] private DataManager _dataManager;
        private Dictionary<PoolType, IObjectPool> _poolDictionary = new Dictionary<PoolType, IObjectPool>(); // pool
        private Dictionary<PoolType, string> _prefabKeyDictionary = new Dictionary<PoolType, string>(); // addressable key
        private Dictionary<PoolType, float> _refTimerDictionary = new Dictionary<PoolType, float>(); // ���� �ð�
        private Dictionary<PoolType, int> _refCountDictionary = new Dictionary<PoolType, int>(); // ���� ī��Ʈ

        private const float LimitTime = 60f;

        private void Awake() {
            AddKey();
        }

        private void Update() {

            // ���� �ð� �����ϸ� ����
            List<PoolType> keysToRemove = new List<PoolType>();

            foreach (var keyValue in _poolDictionary) {
                var poolType = keyValue.Key;
                var timer = _refTimerDictionary[poolType];
                if (timer >= LimitTime) {
                    if (_refCountDictionary[poolType] > 0) continue;
                    _poolDictionary[poolType].Dispose();
                    keysToRemove.Add(poolType);
                } else {
                   _refTimerDictionary[poolType] += Time.deltaTime;
                }
            }

            foreach (var key in keysToRemove) {
                _poolDictionary.Remove(key);
                _refTimerDictionary.Remove(key);
                _refCountDictionary.Remove(key);

                // addressable ����
                string addkey = _prefabKeyDictionary[key];
                _dataManager.ReleaseAsset(addkey);
            }
        }


        public T BorrowItem<T>(PoolType poolType) where T : MonoBehaviour {
            _refTimerDictionary[poolType] = 0f;
            _refCountDictionary[poolType]++;
            return _poolDictionary[poolType].BorrowItem<T>();
        }

        public void Repay(PoolType poolType, GameObject obj) {
            Repay(poolType, obj.GetComponent<ObjectPoolItem>());
        }

        public void Repay(PoolType poolType, ObjectPoolItem poolItem) {
            _refTimerDictionary[poolType] = 0f;
            _refCountDictionary[poolType]--;
            poolItem.Repay();
        }

        // ���
        public void RegisterPool<T>(PoolType poolType) where T : MonoBehaviour {
            if (!_poolDictionary.ContainsKey(poolType)) { // �������� ������ ���
                string key = _prefabKeyDictionary[poolType];
                GameObject prefab = _dataManager.LoadAssetSync<GameObject>(key);
                ObjectPool<T> pool = ObjectPoolBuilder<T>.Instance(prefab).AutoActivate(true).Build();
                _poolDictionary.Add(poolType, pool);
                _refTimerDictionary.Add(poolType, 0);
                _refCountDictionary.Add(poolType, 0);
            }
        }

        // Key ���
        public void AddKey() { // Key
           _prefabKeyDictionary.Add(PoolType.Arrow, "Arrow.prefab");
           _prefabKeyDictionary.Add(PoolType.EnemyL1, "EnemyL1.prefab");
           _prefabKeyDictionary.Add(PoolType.EnemyL2, "EnemyL2.prefab");
           _prefabKeyDictionary.Add(PoolType.EnemyL3, "EnemyL3.prefab");
        } 
    }
}
