using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IExpPolicy 
    {
        // Kill했을때 들어오는 경험치 보상
        public float CalculateKillExperience(EnemyData enemyData);
    }
}
