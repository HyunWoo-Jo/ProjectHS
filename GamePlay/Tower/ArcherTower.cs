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
        [SerializeField] private Transform _bowPivotTr; // bow의 기준점

       

        private LookAtTarget _lookAtTarget;

        protected override void Awake() {
            base.Awake();
#if UNITY_EDITOR
            Assert.IsNotNull(_bowPivotTr);
#endif
            _lookAtTarget = new LookAtTarget(_bowPivotTr);

        }

        private void Start() {
            // pool에 등록 (이미 등록 되어있다면 스킵)
            _poolManager.RegisterPool<ProjectileObject>(PoolType.Arrow);
        }
        private void AimLookAtEnemy() {
            if(targetIndex != -1) { 
                EnemyData enemyData = enemyDataService.GetEnemyData(targetIndex);
                _lookAtTarget.AimLookAtEnemy(enemyData.position);
            }
        }

        public override void AttackLogic() { // 공격이 가능할때 호출 
            curAttackTime = 0; // 시간 초기화
            // 애니메이션 호출
            anim.SetTrigger(ShootAnimHashKey);
            // 화살 소환
            _attackStrategy.Execute(towerData, targetIndex, PoolType.Arrow, _bowPivotTr.transform.position);
        }


        protected override void Update() {
            if (IsPause()) return;
            base.Update();
            AimLookAtEnemy();

        }
    }
}
