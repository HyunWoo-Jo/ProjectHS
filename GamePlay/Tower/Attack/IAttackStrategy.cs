using Data;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    // 공격 처리 정하는 전략
    public interface IAttackStrategy {
        public void Execute(TowerData towerData, int targetIndex, PoolType poolType, float3 startPos);
    }
}
