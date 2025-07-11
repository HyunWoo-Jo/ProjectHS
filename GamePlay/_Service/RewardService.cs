using UnityEngine;
using Contracts;
using Zenject;
using Data;
namespace GamePlay
{
    /// <summary>
    /// Scene���� Catched�� �����Ͽ� Play Scene���� �ѹ��� �۵��ǵ��� ����
    /// </summary>
    public class RewardService : IRewardService {
        [Inject] private IRewardPolicy _rewardPolicy;
        [Inject] private ICrystalRepository _crystalRepo;

        private bool _isProcess = false;
        public int CalculateRewardCrystal() {
            return _rewardPolicy.CalculateCrystalReward();
        }

        public void ProcessFinalRewards() {
            if (_isProcess) return; // �ѹ��� ó���ϵ��� ����
            int reward = _rewardPolicy.CalculateCrystalReward();
            if (reward <= 0) return;
            _crystalRepo.TryEarn(reward);
            _isProcess = true;
        }
    }
}
