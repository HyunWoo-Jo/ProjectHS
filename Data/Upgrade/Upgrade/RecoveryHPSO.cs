using UnityEngine;
using Zenject;

namespace Data
{
    /// <summary>
    /// HP 회복
    /// </summary>
    [CreateAssetMenu(fileName = "RecoveryHPSO", menuName = "Scriptable Objects/Upgrade/RecoveryHPSO")]
    public class RecoveryHPSO : UpgradeStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override void Apply(float value) {
            // 최대 체력을 못넘게 범위 제한
            _hpModel.curHpObservable.Value = Mathf.Min(_hpModel.curHpObservable.Value + (int)value, _hpModel.maxHpObservable.Value);
        }
    }
}
