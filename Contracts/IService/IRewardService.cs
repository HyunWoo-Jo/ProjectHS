using UnityEngine;

namespace Contracts
{
    // ������ ������ ���� ó���� ���ִ� Service
    public interface IRewardService
    {
        public int CalculateRewardCrystal();
        public void ProcessFinalRewards();
    }
}
