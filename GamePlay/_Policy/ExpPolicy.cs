using UnityEngine;
using Data;
namespace GamePlay
{
    public class ExpPolicy : IExpPolicy
    {
        private const float _StartPlayerExp = 0;

        public float CalculateKillExperience(EnemyData enemyData) {
            return 1;
        }

    }
}
