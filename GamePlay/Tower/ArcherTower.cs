using Codice.CM.Common;
using CustomUtility;
using Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace GamePlay
{
    public class ArcherTower : TowerBase {

        [Inject] private GameObjectPoolManager _poolManager;
        [Inject(Id = "Projectile")] private IAttackStrategy _attackStrategy;
        [SerializeField] private Transform _bowPivotTr; // bow�� ������

       

        private LookAtTarget _lookAtTarget;

        protected override void Awake() {
            base.Awake();
#if UNITY_EDITOR
            Assert.IsNotNull(_bowPivotTr);
#endif
            _lookAtTarget = new LookAtTarget(_bowPivotTr);

        }

        private void Start() {
            // pool�� ��� (�̹� ��� �Ǿ��ִٸ� ��ŵ)
            _poolManager.RegisterPool<ProjectileObject>(PoolType.Arrow);
        }
        private void AimLookAtEnemy() {
            if(targetIndex != -1) { 
                EnemyData enemyData = enemyDataService.GetEnemyData(targetIndex);
                _lookAtTarget.AimLookAtEnemy(enemyData.position);
            }
        }

        public override void AttackLogic() { // ������ �����Ҷ� ȣ�� 
            curAttackTime = 0; // �ð� �ʱ�ȭ
            // �ִϸ��̼� ȣ��
            anim.SetTrigger(ShootAnimHashKey);
            // ȭ�� ��ȯ
            _attackStrategy.Execute(towerData, targetIndex, PoolType.Arrow, _bowPivotTr.transform.position);
        }


        protected override void Update() {
            if (IsPause()) return;
            base.Update();
            AimLookAtEnemy();

        }
    }
}
