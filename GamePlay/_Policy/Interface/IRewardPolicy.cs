using UnityEngine;

namespace GamePlay
{
    public interface IRewardPolicy
    {
        /// <summary>
        /// CrystalReward를 계산
        /// </summary>
        /// <returns></returns>
        int CalculateCrystalReward();
    }
}
