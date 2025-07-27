using UnityEngine;
using System.Collections.Generic;
using System;
using Contracts;
namespace Data
{
    [CreateAssetMenu(fileName = "UpgradeDataSO", menuName = "Scriptable Objects/UpgradeDataSO")]
    public class UpgradeDataSO : ScriptableObject, IUpgradeData
    {   
        public Rarity rarity;
        public Sprite sprite;
        public string upgradeName;
        public string description;
        [SerializeField] private List<UnlockModifier> _unlockModifierList = new();
        [SerializeField] private List<UpgradeModifier> _upgradeModifierList = new();


        public int Rarity() => (int)rarity;

        public Sprite Sprite() => sprite;

        public string UpgradeName() => upgradeName;

        public string Description() => description;
        /// <summary>
        /// 업그레이드 적용
        /// </summary>
        public void ApplyUpgrade() {
            foreach (var upgrade in _upgradeModifierList) {
                upgrade.Apply(); // 업그레이드 적용
            }
        }
        /// <summary>
        /// Unlock 되었는지 확인
        /// </summary>
        public bool CheckUnlock() {
            foreach (var unlock in _unlockModifierList) {
                if (!unlock.IsSatisfied()) return false; // 모든 조건이 참일때만 true
            }
            return true;
        }
    }
}
