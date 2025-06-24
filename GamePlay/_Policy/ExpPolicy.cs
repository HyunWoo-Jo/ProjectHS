using UnityEngine;
using Data;
namespace GamePlay
{
    public class ExpPolicy
    {
        private const float _StartPlayerExp = 0;

        public virtual float CalculateKillExperience(EnemyData enemyData) {
            return 1;
        }

    }
}
