using UnityEngine;

namespace GamePlay
{
    public interface IRewardPolicy
    {
        /// <summary>
        /// CrystalReward�� ���
        /// </summary>
        /// <returns></returns>
        int CalculateCrystalReward();
    }
}
