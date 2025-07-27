using UnityEngine;
using Zenject;
using Domain;
using Data;
namespace GamePlay
{
    /// <summary>
    /// HP 회복
    /// </summary>
    [CreateAssetMenu(fileName = "RecoveryHPSO", menuName = "Scriptable Objects/Upgrade/RecoveryHPSO")]
    public class RecoveryHPSO : UpgradeStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override void Apply(float value) {
            _hpModel.RecoverHp((int)value);
        }
    }
}
