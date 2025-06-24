using UnityEngine;
using CustomUtility;
using UnityEditor.Graphs;
using System;
namespace Data
{
    [Serializable]
    public class GlobalUpgradeData {
        [ReadEditor] public int StartPrice; // 시작 비용
        [ReadEditor] public int PriceIncrement; // 단계에 따른 가격 증가량
        [ReadEditor] public int ValueIncrement; // 단계에 따른 수치증가량
    }

    
    [CreateAssetMenu(fileName = "GlobalUpgradeDataSO", menuName = "Scriptable Objects/GlobalUpgradeDataSO")]
    public class GlobalUpgradeDataSO : ScriptableObject
    {
        [ReadEditor] public string Version;
        // Hp
        public GlobalUpgradeData HP;
        public GlobalUpgradeData Power;
        public GlobalUpgradeData InitGold;
        public GlobalUpgradeData Exp;
        private int _hash;

        public int GetPriceIncrement(UpgradeType type) {
            switch (type) {
                case UpgradeType.Power:
                return Power.PriceIncrement;

                case UpgradeType.InitGold:
                return InitGold.PriceIncrement;
                case UpgradeType.Hp:
                return HP.PriceIncrement;
                case UpgradeType.Exp:
                return Exp.PriceIncrement;
            }
            return 0;
        }
        public int GetStartPrice(UpgradeType type) {
            switch (type) {
                case UpgradeType.Power:
                return Power.StartPrice;

                case UpgradeType.InitGold:
                return InitGold.StartPrice;
                case UpgradeType.Hp:
                return HP.StartPrice;
                case UpgradeType.Exp:
                return Exp.StartPrice;
            }
            return 0;
        }

        public int GetValueIncrement(UpgradeType type) {
            switch (type) {
                case UpgradeType.Power:
                return Power.ValueIncrement;

                case UpgradeType.InitGold:
                return InitGold.ValueIncrement;
                case UpgradeType.Hp:
                return HP.ValueIncrement;
                case UpgradeType.Exp:
                return Exp.ValueIncrement;
            }
            return 0;
        }
    }
}
