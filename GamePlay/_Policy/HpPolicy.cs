using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// HP Policy�� �Ѱ����� ����
    /// </summary>
    public class HpPolicy
    {
        private IGlobalUpgradeRepository _globalUpgradeRepo;

        private const int _StartPlayerHp = 20;

        public HpPolicy(IGlobalUpgradeRepository globalUpgradeRepo) {
            _globalUpgradeRepo = globalUpgradeRepo;
        }

        // ���� ���� ������ ���������� �г�Ƽ
        public virtual int CalculateHpPenaltyOnLeak(EnemyData enemyData) {
            return 1;
        }

        public virtual int GetStartPlayerHp() {
            return _StartPlayerHp + _globalUpgradeRepo.GetAbilityValue(UpgradeType.Hp);
        }
    }
}
