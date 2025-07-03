using UnityEngine;

namespace Contracts
{
    // 게임이 끝나면 보상 처리를 해주는 Service
    public interface IRewardService
    {
        public int CalculateRewardCrystal();
        public void ProcessFinalRewards();
    }
}
