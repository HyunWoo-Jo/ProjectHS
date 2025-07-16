using Data;
using UnityEngine;

namespace GamePlay
{
    public interface IExpPolicy 
    {
        // ���� ���� ����ġ Ȯ��
        public int GetNextLevelExp(int level);
        // Kill������ ������ ����ġ ����
        public float CalculateKillExperience(EnemyData enemyData);
    }
}
