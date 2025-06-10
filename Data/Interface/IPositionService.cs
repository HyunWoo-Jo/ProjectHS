using Unity.Mathematics;
using UnityEngine;

namespace Data
{
    public interface IPositionService
    {
        float3 GetIndexToWorldPosition(int index);
    }
}
