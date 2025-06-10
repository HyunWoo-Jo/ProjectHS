using Codice.CM.Client.Differences.Graphic;
using Data;
using UnityEditor.EditorTools;
using UnityEngine;
using Unity.Mathematics;
using Zenject;

namespace GamePlay
{
    public class ProjectileAttackStrategy : IAttackStrategy
    {
        [Inject] private GameObjectPoolManager _poolManager;
        [Inject] private IEnemyDataService enemyDataService;

        public void Execute(TowerData towerData, int targetIndex, PoolType poolType, float3 startPos) {
            EnemyData enemyData = enemyDataService.GetEnemyData(targetIndex);
            if (!enemyData.isDead) { // 살아있을 경우
                ProjectileObject projectile = _poolManager.BorrowItem<ProjectileObject>(poolType); // 투사체 생성
                // 임시 데미지 처리
                enemyData.nextTempHp -= towerData.attackPower;
                enemyDataService.SetEnemyData(targetIndex, enemyData);
                projectile.SetTarget(startPos, enemyData.position, () => { // arrow 목표 지점에 도착시 컨트롤 하는 로직
                    // 도착시 데미지 처리
                    EnemyData temp = enemyDataService.GetEnemyData(targetIndex);
                    if (!temp.isDead) {
                        temp.curHp = temp.nextTempHp;
                        enemyDataService.SetEnemyData(targetIndex, temp);
                    }
                    _poolManager.Repay(poolType, projectile.gameObject);
                });
            }
        }
    }
}
