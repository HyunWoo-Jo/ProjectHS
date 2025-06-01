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
        [SerializeField] private Transform _bowPivotTr; // bow�� ������

        [SerializeField] private Animator _bowAnim;
        private static int _shootAnimHashKey = -1;
        private const float _RotationSpeed = 30f;

        protected override void Awake() {
            base.Awake();
#if UNITY_EDITOR
            Assert.IsNotNull(_bowPivotTr);
#endif
            // pool�� ��� (�̹� ��� �Ǿ��ִٸ� ��ŵ)
            _poolManager.RegisterPool<ProjectileObject>(PoolType.Arrow);

            if (_shootAnimHashKey == -1) {
                _shootAnimHashKey = Animator.StringToHash("Shoot");
            }

        }
        private void AimLookAtEnemy() {
            if(targetIndex != -1) { 
                
                EnemyData enemyData = enemyDataService.GetEnemyData(targetIndex);
                float3 dir = math.normalize(enemyData.position - (float3)_bowPivotTr.position);
                dir.z = 0; // z�� ����
                if (math.lengthsq(dir) > 0.0001f) {
                    dir = math.normalize(dir);

                    quaternion targetRot = quaternion.LookRotationSafe(dir, math.up()); // �⺻ ȸ��
                    quaternion newRot = math.slerp(_bowPivotTr.rotation, targetRot, Time.deltaTime * _RotationSpeed);
                    float3 euler = math.degrees(math.EulerXYZ(targetRot));
                     euler.y = euler.y > 0 ? 90 : -90; // ���� ����
        
                    _bowPivotTr.eulerAngles = euler;
                }
            }
        }

        public override void AttackLogic() { // ������ �����Ҷ� ȣ�� 
            curAttackTime = 0; // �ð� �ʱ�ȭ

            _bowAnim.SetTrigger(_shootAnimHashKey);

            EnemyData enemyData = enemyDataService.GetEnemyData(targetIndex);
            // arrow ����
            ProjectileObject arrow = _poolManager.BorrowItem<ProjectileObject>(PoolType.Arrow);
            arrow.SetTarget(_bowPivotTr.transform.position, enemyData.position, () => { // arrow ��ǥ ������ ������ ��Ʈ�� �ϴ� ����
                _poolManager.Repay(PoolType.Arrow, arrow.gameObject);

            });
            


        }


        protected override void Update() {
            base.Update();
            AimLookAtEnemy();

        }

        public override void SetAttackSpeed(float speed) {
            _bowAnim.speed = speed;
        }
    }
}
