using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    public class StandardWaveStrategy : IWaveStrategy {
        public SpawnData GetSpawnData(int stageLevel, float3 spawnPosition) {
            SpawnData spawnData = new SpawnData();

            float hp = stageLevel * 10;
            EnemyData enemyData = new EnemyData {
                position = spawnPosition,
                curHp = hp,
                maxHp = hp,
                speed = 1,
                isSpawn = false,
                isDead = false,
                currentPathIndex = 0
            };
            spawnData.enemyData = enemyData;
            spawnData.spawnInterval = 1;
            spawnData.spawnCount = stageLevel * 20;
            spawnData.spawnEnemyPoolType = PoolType.EnemyL1;
            return spawnData;
        }
    }
}
