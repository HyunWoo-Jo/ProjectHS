using UnityEngine;
using Zenject;

namespace Data
{
    /// <summary>
    /// ���� HP�� Ư�� HP �Ʒ��ִ��� üũ
    /// </summary>
    [CreateAssetMenu(fileName = "UnlockHpUnderSO", menuName = "Scriptable Objects/Unlock/UnlockHpUnderSO")]
    public class UnlockHpUnderSO : UnlockStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override bool IsSatisfied(float value) {
           return _hpModel.curHpObservable.Value < (int)value;
        }
    }
}
