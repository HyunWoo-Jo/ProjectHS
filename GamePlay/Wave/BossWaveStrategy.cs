using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    public class BossWaveStrategy : IWaveStrategy {
        public SpawnData GetSpawnData(int stageLevel, float3 spawnPosition) {
            return new SpawnData();
        }
    }
}
