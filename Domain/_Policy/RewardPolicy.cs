using Domain;
using UnityEngine;
using Zenject;

namespace Domain {
    public class RewardPolicy : IRewardPolicy {

        public int CalculateCrystalReward(int level) {
            return level / 10; // 10스테이지 마다 1 Crystal 보상
        }
    }
}
