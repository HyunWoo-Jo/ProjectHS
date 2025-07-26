using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IHpPolicy
    {
        // 적이 최종 지점에 도달했을때 패널티
        public int CalculateHpPenaltyOnLeak(EnemyData enemyData);
    
        // 시작 HP
        public int GetStartPlayerHp();
    }
}
