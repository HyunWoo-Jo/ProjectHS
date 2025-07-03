using UnityEngine;
using Contracts;
using Zenject;
using Data;
namespace GamePlay
{
    public class RewardService : IRewardService {
        [Inject] private IRewardPolicy _rewardPolicy;
        [Inject] private ICrystalRepository _crystalRepo;

        private bool _isProcess = false;
        public int CalculateRewardCrystal() {
            return _rewardPolicy.CalculateCrystalReward();
        }

        public void ProcessFinalRewards() {
            if (_isProcess) return; // 한번만 처리하도록 설정
            int reward = _rewardPolicy.CalculateCrystalReward();
            if (reward <= 0) return;
            _crystalRepo.SetValue(_crystalRepo.GetValue() + reward);
            _isProcess = true;
        }
    }
}
