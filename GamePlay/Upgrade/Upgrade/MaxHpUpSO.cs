using UnityEngine;
using Zenject;
using Domain;
using Data;
namespace GamePlay {
    /// <summary>
    /// 최대 체력을 높여줌
    /// </summary>
    [CreateAssetMenu(fileName = "MaxHpUpSO", menuName = "Scriptable Objects/Upgrade/MaxHpUpSO")]
    public class MaxHpUpSO : UpgradeStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override void Apply(float value) {
            _hpModel.SetMaxHp(_hpModel.CurHp + (int)value);
        }
    }
}
