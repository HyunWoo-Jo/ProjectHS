using UnityEngine;
using CustomUtility;
using UnityEngine.Assertions;
using Unity.Mathematics;
using Data;
using DG.Tweening;
using System;
namespace GamePlay
{
    public class GoldDropper : MonoBehaviour
    {  
        // Gold Image UI Pool
        private ObjectPool<ObjectPoolItem> _goldPool;
        [SerializeField] private GameObject _goldPrefab;
        [SerializeField] private Transform _targetTr;
        public Action<EnemyData> OnArrived; // 도착 했을때 발동
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_goldPrefab);
            Assert.IsNotNull(_targetTr);
#endif
            // 시작시 Pool생성 Gold Prefab
            _goldPool = ObjectPoolBuilder<ObjectPoolItem>.Instance(_goldPrefab, 10).AutoActivate(true).Parent(this.gameObject.transform).Build(); // pool 생성
        }

        // 생성 후 이동
        public void SpawnAndMoveToTarget(EnemyData enemyData) {
            var poolItem = _goldPool.BorrowItem();
            // UI World -> Screen Postion 변환 
            poolItem.transform.position = Camera.main.WorldToScreenPoint(enemyData.position);
            // 이동
            poolItem.transform.DOMove(_targetTr.position, 1f).OnComplete(() => {
                // 도착시
                _goldPool.RepayItem(poolItem); // 반환
                OnArrived?.Invoke(enemyData); // Event 발생
            });
        }

        
    }
}
