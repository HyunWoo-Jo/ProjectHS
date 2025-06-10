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
            if (!enemyData.isDead) { // ������� ���
                ProjectileObject projectile = _poolManager.BorrowItem<ProjectileObject>(poolType); // ����ü ����
                // �ӽ� ������ ó��
                enemyData.nextTempHp -= towerData.attackPower;
                enemyDataService.SetEnemyData(targetIndex, enemyData);
                projectile.SetTarget(startPos, enemyData.position, () => { // arrow ��ǥ ������ ������ ��Ʈ�� �ϴ� ����
                    // ������ ������ ó��
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
