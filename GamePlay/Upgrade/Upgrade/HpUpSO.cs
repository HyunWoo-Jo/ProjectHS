using UnityEngine;
using Zenject;
using Domain;
using Data;
namespace GamePlay
{
    /// <summary>
    /// 현재 체력을 높여줌   
    /// </summary>
    [CreateAssetMenu(fileName = "HpUpSO", menuName = "Scriptable Objects/Upgrade/HpUpSO")]
    public class HpUpSO : UpgradeStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override void Apply(float value) {
            _hpModel.SetCurHp(_hpModel.CurHp + (int)value);
        }
    }
}
