using UnityEngine;
using Contracts;
using Zenject;
using Data;
using Domain;
namespace GamePlay
{
    /// <summary>
    /// Scene에서 Catched로 관리하여 Play Scene에서 한번만 작동되도록 관리
    /// </summary>
    public class RewardService : IRewardService {
        [Inject] private IRewardPolicy _rewardPolicy;
        [Inject] private CrystalModel _crystalModel;
        [Inject] private WaveStatusModel _waveModel;

        public int CalculateRewardCrystal() {
            return _rewardPolicy.CalculateCrystalReward(_waveModel.WaveLevel);
        }

        public void ProcessFinalRewards() {
            int reward = CalculateRewardCrystal();
            if (reward <= 0) return;
            _ = _crystalModel.TryEarn(reward);
        }
    }
}
