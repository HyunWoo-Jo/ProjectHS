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
        SpawnData GetSpawnData(int stageLevel, float3 spawnPosition, float spawnTimeout);
    }

}
