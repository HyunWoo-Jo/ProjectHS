using UnityEngine;

namespace GamePlay
{
    public interface IRewardPolicy
    {
        /// <summary>
        /// CrystalReward¸¦ °è»ê
        /// </summary>
        /// <returns></returns>
        int CalculateCrystalReward();
    }
}
