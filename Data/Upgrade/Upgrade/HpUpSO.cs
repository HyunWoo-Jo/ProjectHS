using UnityEngine;
using Zenject;

namespace Data
{
    /// <summary>
    /// 현재 체력을 높여줌   
    /// </summary>
    [CreateAssetMenu(fileName = "HpUpSO", menuName = "Scriptable Objects/Upgrade/HpUpSO")]
    public class HpUpSO : UpgradeStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override void Apply(float value) {
            _hpModel.curHpObservable.Value += (int)value;
        }
    }
}
