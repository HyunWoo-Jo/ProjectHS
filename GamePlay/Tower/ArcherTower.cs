using CustomUtility;
using Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace GamePlay
{
    public class ArcherTower : TowerBase {

        [SerializeField] private Transform _bowPivotTr;
        
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_bowPivotTr);
#endif
        }
        private void AimLookAtEnemy() {
            if(targetIndex != -1) { 
                EnemyData enemyData = enemyDataService.GetEnemyData(targetIndex);
                _bowPivotTr.transform.LookAt(enemyData.position);
            }
        }
        
        public override void Attack(EnemyData enemyData) {
            
        }

        public override void SetAttackSpeed(float speed) {
            throw new System.NotImplementedException();
        }

        public override void SetPosition(Vector2 pos) {
            throw new System.NotImplementedException();
        }

        protected override void Update() {
            base.Update();
            AimLookAtEnemy();

        }
    }
}
