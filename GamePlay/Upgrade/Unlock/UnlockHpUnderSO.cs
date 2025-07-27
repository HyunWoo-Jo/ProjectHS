using UnityEngine;
using Zenject;
using Domain;
using Data;
namespace GamePlay
{
    /// <summary>
    /// 현재 HP가 특정 HP 아래있는지 체크
    /// </summary>
    [CreateAssetMenu(fileName = "UnlockHpUnderSO", menuName = "Scriptable Objects/Unlock/UnlockHpUnderSO")]
    public class UnlockHpUnderSO : UnlockStrategyBaseSO {
        [Inject] private HpModel _hpModel;
        public override bool IsSatisfied(float value) {
           return _hpModel.CurHp < (int)value;
        }
    }
}
