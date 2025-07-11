using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// HP Policy를 한곳에서 관리
    /// </summary>
    public class HpPolicy : IHpPolicy
    {
        private IGlobalUpgradeRepository _globalUpgradeRepo;

        private const int _StartPlayerHp = 20;

        public HpPolicy(IGlobalUpgradeRepository globalUpgradeRepo) {
            _globalUpgradeRepo = globalUpgradeRepo;
        }

        // 적이 최종 지점에 도달했을때 패널티
        public int CalculateHpPenaltyOnLeak(EnemyData enemyData) {
            return 1;
        }

        public int GetStartPlayerHp() {
            return _StartPlayerHp + _globalUpgradeRepo.GetAbilityValue(GlobalUpgradeType.Hp);
        }
    }
}
