using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IHpPolicy
    {
        // ���� ���� ������ ���������� �г�Ƽ
        public int CalculateHpPenaltyOnLeak(EnemyData enemyData);
    
        // ���� HP
        public int GetStartPlayerHp();
    }
}
