using UnityEngine;
using System.Collections.Generic;
using System;
namespace Data
{
    [CreateAssetMenu(fileName = "UpgradeDataSO", menuName = "Scriptable Objects/UpgradeDataSO")]
    public class UpgradeDataSO : ScriptableObject
    {   
        public Rarity rarity;
        public Sprite sprite;
        public string upgradeName;
        public string description;
        [SerializeField] private List<UnlockModifier> _unlockModifierList = new();
        [SerializeField] private List<UpgradeModifier> _upgradeModifierList = new();

        /// <summary>
        /// Unlock 되었는지 확인
        /// </summary>
        /// <returns></returns>
        public bool CheckUnlock() {
            foreach(var unlock in _unlockModifierList) {
                if (!unlock.IsSatisfied()) return false; // 모든 조건이 참일때만 true
            }
            return true;
        }

        /// <summary>
        /// 업그레이드 적용
        /// </summary>
        public void ApplyUpgrade() {

            foreach(var upgrade in _upgradeModifierList) {
                upgrade.Apply(); // 업그레이드 적용
            }
        }

    }
}
