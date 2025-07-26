using Data;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class RewardPolicy : IRewardPolicy {
        [Inject] private WaveStatusModel _waveModel;

        public int CalculateCrystalReward() {
            return _waveModel.waveLevelObservable.Value / 10; // 10스테이지 마다 1 Crystal 보상
        }
    }
}
