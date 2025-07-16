using UnityEngine;
using Data;
namespace GamePlay
{
    public class ExpPolicy : IExpPolicy
    {
        private const float _StartPlayerExp = 0;

        public int GetNextLevelExp(int level) {
            return (level + 1) * 20;
        }

        public float CalculateKillExperience(EnemyData enemyData) {
            return 1;
        }

    }
}
