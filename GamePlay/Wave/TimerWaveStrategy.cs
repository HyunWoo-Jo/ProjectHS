using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    public class TimerWaveStrategy : IWaveStrategy {
        public SpawnData GetSpawnData(int stageLevel, float3 spawnPosition, float spawnTimeout) {
            return new SpawnData();
        }
    }

}
