using UnityEngine;
using Zenject;
using Domain;
using Data;
namespace GamePlay
{

    /// <summary>
    /// 특정 스테이지 아래 있나 체크
    /// </summary>
    [CreateAssetMenu(fileName = "UnlockStageUnderSO", menuName = "Scriptable Objects/Unlock/UnlockStageUnderSO")]
    public class UnlockStageUnderSO : UnlockStrategyBaseSO {
        [Inject] private WaveStatusModel _waveModel;
        public override bool IsSatisfied(float value) {
            return _waveModel.WaveLevel < (int)value;
        }
    }
}
