using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    /// <summary>
    /// Stage에 따라 어떤 Wave를 발생할건지 정하는 전략
    /// </summary>
    public interface IWaveStrategy
    {
        SpawnData GetSpawnData(int stageLevel, float3 spawnPosition);
    }

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
            spawnData.spawnCount = stageLevel;
            spawnData.spawnEnemyKey = EnemyPrefabKey.EnemyL1.ToString() + ".prefab";
            return spawnData;
        }
    }

    public class TimerWaveStrategy : IWaveStrategy {
        public SpawnData GetSpawnData(int stageLevel, float3 spawnPosition) {
            return new SpawnData();
        }
    }

    public class BossWaveStrategy : IWaveStrategy {
        public SpawnData GetSpawnData(int stageLevel, float3 spawnPosition) {
            return new SpawnData();
        }
    }
}
