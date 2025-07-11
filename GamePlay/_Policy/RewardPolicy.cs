using Data;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class RewardPolicy : IRewardPolicy {
        [Inject] private WaveStatusModel _waveModel;

        public int CalculateCrystalReward() {
            return _waveModel.waveLevelObservable.Value / 10; // 10�������� ���� 1 Crystal ����
        }
    }
}
