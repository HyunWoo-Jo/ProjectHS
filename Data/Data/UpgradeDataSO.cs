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
        /// Unlock �Ǿ����� Ȯ��
        /// </summary>
        /// <returns></returns>
        public bool CheckUnlock() {
            foreach(var unlock in _unlockModifierList) {
                if (!unlock.IsSatisfied()) return false; // ��� ������ ���϶��� true
            }
            return true;
        }

        /// <summary>
        /// ���׷��̵� ����
        /// </summary>
        public void ApplyUpgrade() {

            foreach(var upgrade in _upgradeModifierList) {
                upgrade.Apply(); // ���׷��̵� ����
            }
        }

    }
}
