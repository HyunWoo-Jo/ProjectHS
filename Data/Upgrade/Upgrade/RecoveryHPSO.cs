using UnityEngine;
using Zenject;

namespace Data
{
    /// <summary>
    /// HP ȸ��
    /// </summary>
    [CreateAssetMenu(fileName = "RecoveryHPSO", menuName = "Scriptable Objects/Upgrade/RecoveryHPSO")]
    public class RecoveryHPSO : UpgradeStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override void Apply(float value) {
            // �ִ� ü���� ���Ѱ� ���� ����
            _hpModel.curHpObservable.Value = Mathf.Min(_hpModel.curHpObservable.Value + (int)value, _hpModel.maxHpObservable.Value);
        }
    }
}
