using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    /// <summary>
    /// Stage�� ���� � Wave�� �߻��Ұ��� ���ϴ� ����
    /// </summary>
    public interface IWaveStrategy
    {
        SpawnData GetSpawnData(int stageLevel, float3 spawnPosition);
    }

}
