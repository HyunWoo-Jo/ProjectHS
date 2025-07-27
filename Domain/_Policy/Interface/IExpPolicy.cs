using UnityEngine;

namespace Domain
{
    public interface IExpPolicy 
    {
        // 다음 레벨 경험치 확인
        int GetNextLevelExp(int level);
        float CalculateExp(float exp);
    }
}
