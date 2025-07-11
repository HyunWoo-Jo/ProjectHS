using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    public class StandardWaveStrategy : IWaveStrategy {
        public SpawnData GetSpawnData(int stageLevel, float3 spawnPosition, float spawnTimeout) {
            SpawnData spawnData = new SpawnData();

            int hp = stageLevel;
            int spawnCount = stageLevel * 3;
            EnemyData enemyData = new EnemyData {
                position = spawnPosition,
                curHp = hp,
                maxHp = hp,
                nextTempHp = hp,
                speed = 1,
                isSpawn = false,
                isDead = false,
                currentPathIndex = 0
            };
            spawnData.enemyData = enemyData;
            spawnData.spawnInterval = spawnTimeout / spawnCount;
            spawnData.spawnCount = spawnCount;
            spawnData.spawnEnemyPoolType = PoolType.EnemyL1;
            return spawnData;
        }
    }
}
