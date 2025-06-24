using UnityEngine;
using Data;
using Zenject;
namespace GamePlay
{
    // 인게임에 사용되는 골드 정보를 저장하는 클레스
    public class GoldPolicy
    {
        private IGlobalUpgradeRepository _globalUpgradeRepo;
        private const int _DefaultStartGold = 10; // 시작 골드

        public GoldPolicy(IGlobalUpgradeRepository globalUpgradeRepo) {
            _globalUpgradeRepo = globalUpgradeRepo;
        }

        public virtual int CalculateKillReward(EnemyData enemyData) {
            return 1;
        }

        public virtual int GetPlayerStartGold() {
            return _DefaultStartGold + _globalUpgradeRepo.GetAbilityValue(UpgradeType.InitGold); 
        }
    }
}
