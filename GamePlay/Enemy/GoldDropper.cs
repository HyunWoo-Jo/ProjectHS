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
        public Action<EnemyData> OnArrived; // ���� ������ �ߵ�
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_goldPrefab);
            Assert.IsNotNull(_targetTr);
#endif
            // ���۽� Pool���� Gold Prefab
            _goldPool = ObjectPoolBuilder<ObjectPoolItem>.Instance(_goldPrefab, 10).AutoActivate(true).Parent(this.gameObject.transform).Build(); // pool ����
        }

        // ���� �� �̵�
        public void SpawnAndMoveToTarget(EnemyData enemyData) {
            var poolItem = _goldPool.BorrowItem();
            // UI World -> Screen Postion ��ȯ 
            poolItem.transform.position = Camera.main.WorldToScreenPoint(enemyData.position);
            // �̵�
            poolItem.transform.DOMove(_targetTr.position, 1f).OnComplete(() => {
                // ������
                _goldPool.RepayItem(poolItem); // ��ȯ
                OnArrived?.Invoke(enemyData); // Event �߻�
            });
        }

        
    }
}
