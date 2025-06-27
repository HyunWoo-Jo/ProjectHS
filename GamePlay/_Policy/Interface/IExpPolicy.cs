using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IExpPolicy 
    {
        // Kill������ ������ ����ġ ����
        public float CalculateKillExperience(EnemyData enemyData);
    }
}
