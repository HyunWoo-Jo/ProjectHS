using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IGoldPolicy
    {
        // enemy Kill������ ��� ����
        public int CalculateKillReward(EnemyData enemyData);

        // ���� ���
        public int GetPlayerStartGold();
    }
}
