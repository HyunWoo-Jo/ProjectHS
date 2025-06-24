using UnityEngine;
using Data;
using Zenject;
namespace GamePlay
{
    // �ΰ��ӿ� ���Ǵ� ��� ������ �����ϴ� Ŭ����
    public class GoldPolicy
    {
        private IGlobalUpgradeRepository _globalUpgradeRepo;
        private const int _DefaultStartGold = 10; // ���� ���

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
