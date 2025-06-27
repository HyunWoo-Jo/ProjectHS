using UnityEngine;
using Data;
using Zenject;
namespace GamePlay
{
    // �ΰ��ӿ� ���Ǵ� ��� ������ �����ϴ� Ŭ����
    public class GoldPolicy : IGoldPolicy
    {
        private IGlobalUpgradeRepository _globalUpgradeRepo;
        private const int _DefaultStartGold = 10; // ���� ���

        [Inject]
        public GoldPolicy(IGlobalUpgradeRepository globalUpgradeRepo) {
            _globalUpgradeRepo = globalUpgradeRepo;
        }

        public int CalculateKillReward(EnemyData enemyData) {
            return 1;
        }

        public int GetPlayerStartGold() {
            return _DefaultStartGold + _globalUpgradeRepo.GetAbilityValue(UpgradeType.InitGold); 
        }
    }
}
