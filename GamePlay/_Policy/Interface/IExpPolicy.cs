using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IExpPolicy 
    {
        // 다음 레벨 경험치 확인
        public int GetNextLevelExp(int level);
        // Kill했을때 들어오는 경험치 보상
        public float CalculateKillExperience(EnemyData enemyData);
    }
}
