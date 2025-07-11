using UnityEngine;

namespace Data
{
    public class SpawnData
    {
        public EnemyData enemyData;
        public int spawnCount;
        public float spawnInterval;

        public float curRemainingTime = 0;
        public int curSpawnIndex = 0;
        public int startIndex = 0;

        public PoolType spawnEnemyPoolType;
    }
}
