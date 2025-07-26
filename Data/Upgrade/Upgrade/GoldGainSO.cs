using UnityEngine;
using Zenject;

namespace Data
{
    /// <summary>
    /// 골드 획득
    /// </summary>
    [CreateAssetMenu(fileName = "GoldGainSO", menuName = "Scriptable Objects/Upgrade/GoldGainSO")]
    public class GoldGainSO : UpgradeStrategyBaseSO {
        [Inject] private GoldModel _goldModel;
        public override void Apply(float value) {
            _goldModel.goldObservable.Value += (int)value;
        }
    }
}
