using UnityEngine;
using Zenject;

namespace Data
{
    /// <summary>
    /// Ư�� �������� ���� �ֳ�
    /// </summary>
    [CreateAssetMenu(fileName = "UnlockStageUpperSO", menuName = "Scriptable Objects/Unlock/UnlockStageUpperSO")]
    public class UnlockStageUpperSO : UnlockStrategyBaseSO {
        [Inject] private WaveStatusModel _waveModel;
        public override bool IsSatisfied(float value) {
            return _waveModel.waveLevelObservable.Value > (int)value;
        }
    }
}
