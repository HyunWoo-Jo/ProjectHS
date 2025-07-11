using UnityEngine;
using System.Collections.Generic;
using CustomUtility;
using Zenject;
namespace Data
{


    /// <summary>
    /// ObjectPool들을 관리하는 Manager
    /// </summary>
    public class GameObjectPoolManager : MonoBehaviour
    {
        [Inject] private DataManager _dataManager;
        [Inject] private DiContainer _diContainer;
        private Dictionary<PoolType, IObjectPool> _poolDictionary = new Dictionary<PoolType, IObjectPool>(); // pool
        private Dictionary<PoolType, string> _prefabKeyDictionary = new Dictionary<PoolType, string>(); // addressable key
        private Dictionary<PoolType, float> _refTimerDictionary = new Dictionary<PoolType, float>(); // 참조 시간
        private Dictionary<PoolType, int> _refCountDictionary = new Dictionary<PoolType, int>(); // 참조 카운트

        private const float LimitTime = 60f;

        private void Awake() {
            AddKey();
        }
        private void OnDestroy() {
            // 로드된 오브젝트 모드 반환
            foreach (var keyValue in _prefabKeyDictionary) {
                _dataManager.ReleaseAsset(keyValue.Value);
            }
        }
        private void Update() {

            // 일정 시간 사용안하면 삭제
            foreach (var keyValue in _poolDictionary) {
                var poolType = keyValue.Key;
                var timer = _refTimerDictionary[poolType];
                if (timer >= LimitTime) {
                    if (_refCountDictionary[poolType] > 0) continue;
                    _poolDictionary[poolType].Clear();
                } else {
                   _refTimerDictionary[poolType] += Time.deltaTime;
                }
            }
        }


        public T BorrowItem<T>(PoolType poolType) where T : MonoBehaviour {
            if(!_poolDictionary.ContainsKey(poolType)) RegisterPool<T>(poolType); 
            _refTimerDictionary[poolType] = 0f;
            _refCountDictionary[poolType]++;
            T item = _poolDictionary[poolType].BorrowItem<T>();
            _diContainer.InjectGameObject(item.gameObject); // 의존 주입
            return item;
        }

        public void Repay(PoolType poolType, GameObject obj) {
            Repay(poolType, obj.GetComponent<ObjectPoolItem>());
        }

        public void Repay(PoolType poolType, ObjectPoolItem poolItem) {
            _refTimerDictionary[poolType] = 0f;
            _refCountDictionary[poolType]--;
            poolItem.Repay();
        }

        // 등록
        public void RegisterPool<T>(PoolType poolType, Transform parentTr = null) where T : MonoBehaviour {
            if (!_poolDictionary.ContainsKey(poolType)) { // 존재하지 않으면 등록
                string key = _prefabKeyDictionary[poolType];
                GameObject prefab = _dataManager.LoadAssetSync<GameObject>(key);
                var builder = ObjectPoolBuilder<T>.Instance(prefab).AutoActivate(true);
                if (parentTr != null) { builder.Parent(parentTr); }
                ObjectPool<T> pool = builder.Build();
                _poolDictionary.Add(poolType, pool);
                _refTimerDictionary.Add(poolType, 0);
                _refCountDictionary.Add(poolType, 0);
            }
        }

        // Key 등록
        public void AddKey() { // Key
           _prefabKeyDictionary.Add(PoolType.Arrow, "Arrow.prefab");
           _prefabKeyDictionary.Add(PoolType.MagicBullet, "MagicBullet.prefab");
           _prefabKeyDictionary.Add(PoolType.EnemyL1, "EnemyL1.prefab");
           _prefabKeyDictionary.Add(PoolType.EnemyL2, "EnemyL2.prefab");
           _prefabKeyDictionary.Add(PoolType.EnemyL3, "EnemyL3.prefab");
           _prefabKeyDictionary.Add(PoolType.DamageLogUI, "DamageLogUI_Text.prefab");
        } 
    }
}
