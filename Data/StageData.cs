using UnityEngine;

namespace Data
{
    public enum StageType {
        Standard,
        Boss,
        Timer,
    }

    public class StageData 
    {
        public int spawnCount;
        public EnemyData enemyData;
        public GameObject enemyPrefab;
    }
}
