using Data;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;


namespace GamePlay
{
    public class MageTower : TowerBase {

        [Inject] private GameObjectPoolManager _poolManager;
        [Inject(Id = "Projectile")] private IAttackStrategy _attackStrategy;
        [SerializeField] private Transform _stickPivotTr; // stick�� ������

        private LookAtTarget _lookAtTarget;

        protected override void Awake() {
            base.Awake();
#if UNITY_EDITOR       
            Assert.IsNotNull(_stickPivotTr);
#endif
            _lookAtTarget = new LookAtTarget(_stickPivotTr);

        }

        private void Start() {
            // pool�� ��� (�̹� ��� �Ǿ��ִٸ� ��ŵ)
            _poolManager.RegisterPool<ProjectileObject>(PoolType.MagicBullet);
        }
        private void AimLookAtEnemy() {
            if (targetIndex != -1) {
                EnemyData enemyData = enemyDataStore.GetEnemyData(targetIndex);
                _lookAtTarget.AimLookAtEnemy(enemyData.position);
            }
        }

        public override void AttackLogic() { // ������ �����Ҷ� ȣ�� 
            curAttackTime = 0; // �ð� �ʱ�ȭ
            // �ִϸ��̼� ȣ��
            anim.SetTrigger(ShootAnimHashKey);
            // ȭ�� ��ȯ
            _attackStrategy.Execute(towerData, targetIndex, PoolType.MagicBullet, _stickPivotTr.transform.position);
        }


        protected override void Update() {
            if (IsPause()) return;
            base.Update();
            AimLookAtEnemy();

        }

    }
}
