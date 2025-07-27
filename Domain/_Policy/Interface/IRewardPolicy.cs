using UnityEngine;

namespace Domain {
    public interface IRewardPolicy
    {
        /// <summary>
        /// CrystalReward를 계산
        /// </summary>
        int CalculateCrystalReward(int level);
    }
}
