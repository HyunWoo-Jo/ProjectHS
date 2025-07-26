using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IGoldPolicy
    {
        // enemy Kill했을때 골드 보상
        public int CalculateKillReward(EnemyData enemyData);

        // 시작 골드
        public int GetPlayerStartGold();
    }
}
