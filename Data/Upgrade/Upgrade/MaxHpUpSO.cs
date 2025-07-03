using UnityEngine;
using Zenject;

namespace Data
{
    /// <summary>
    /// �ִ� ü���� ������
    /// </summary>
    [CreateAssetMenu(fileName = "MaxHpUpSO", menuName = "Scriptable Objects/Upgrade/MaxHpUpSO")]
    public class MaxHpUpSO : UpgradeStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override void Apply(float value) {
            _hpModel.maxHpObservable.Value += (int)value;
        }
    }
}
