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
        private ObjectPool<ObjectPoolItem> _goldPool;
        [SerializeField] private GameObject _goldPrefab;
        [SerializeField] private Transform _targetTr;
        public Action<EnemyData> OnArrived; // 도착 했을때 발동
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_goldPrefab);
            Assert.IsNotNull(_targetTr);
#endif
            _goldPool = ObjectPoolBuilder<ObjectPoolItem>.Instance(_goldPrefab, 10).AutoActivate(true).Parent(this.gameObject.transform).Build(); // pool 생성
        }

        public void SpawnAndMoveToTarget(EnemyData enemyData) {
            var poolItem = _goldPool.BorrowItem();
            poolItem.transform.position = Camera.main.WorldToScreenPoint(enemyData.position);
            poolItem.transform.DOMove(_targetTr.position, 1f).OnComplete(() => {
                _goldPool.RepayItem(poolItem);
                OnArrived?.Invoke(enemyData);
            });

        }
    }
}
