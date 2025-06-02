using Unity.Mathematics;
using UnityEngine;

namespace Data
{
    public interface IPositionService
    {
        float3 GetGridToWorldPosition(int index);
    }
}
