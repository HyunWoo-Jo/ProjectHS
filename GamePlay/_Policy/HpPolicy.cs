using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// HP Policy�� �Ѱ����� ����
    /// </summary>
    public class HpPolicy : IHpPolicy
    {
        private IGlobalUpgradeRepository _globalUpgradeRepo;

        private const int _StartPlayerHp = 20;

        public HpPolicy(IGlobalUpgradeRepository globalUpgradeRepo) {
            _globalUpgradeRepo = globalUpgradeRepo;
        }

        // ���� ���� ������ ���������� �г�Ƽ
        public int CalculateHpPenaltyOnLeak(EnemyData enemyData) {
            return 1;
        }

        public int GetStartPlayerHp() {
            return _StartPlayerHp + _globalUpgradeRepo.GetAbilityValue(GlobalUpgradeType.Hp);
        }
    }
}
