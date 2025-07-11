using Codice.CM.Client.Differences.Graphic;
using Data;
using UnityEditor.EditorTools;
using UnityEngine;
using Unity.Mathematics;
using Zenject;
using UI;

namespace GamePlay
{
    public class ProjectileAttackStrategy : IAttackStrategy
    {
        [Inject] private GameObjectPoolManager _poolManager;
        [Inject] private IEnemyDataStore enemyDataStore;

        public void Execute(TowerData towerData, int targetIndex, PoolType poolType, float3 startPos) {
            EnemyData enemyData = enemyDataStore.GetEnemyData(targetIndex);
            if (!enemyData.isDead) { // 살아있을 경우
                ProjectileObject projectile = _poolManager.BorrowItem<ProjectileObject>(poolType); // 투사체 생성
                // 임시 데미지 처리
                int damage = towerData.attackPower;
                enemyData.nextTempHp -= damage;
                enemyDataStore.SetEnemyData(targetIndex, enemyData);
                projectile.SetTarget(startPos, enemyData.position, () => { // arrow 목표 지점에 도착시 컨트롤 하는 로직
                    // 도착시 데미지 처리
                    EnemyData temp = enemyDataStore.GetEnemyData(targetIndex);
                    DamageLogUI log = _poolManager.BorrowItem<DamageLogUI>(PoolType.DamageLogUI);
                    log.SetWorldToScreenPosition(temp.position);
                    log.SetDamage(damage);
                    if (!temp.isDead) {
                        temp.curHp = temp.nextTempHp;
                        enemyDataStore.SetEnemyData(targetIndex, temp);
                        
                    }
                    _poolManager.Repay(poolType, projectile.gameObject);
                });
            }
        }
    }
}
