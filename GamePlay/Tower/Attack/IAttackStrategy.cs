using Data;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    // ���� ó�� ���ϴ� ����
    public interface IAttackStrategy {
        public void Execute(TowerData towerData, int targetIndex, PoolType poolType, float3 startPos);
    }
}
