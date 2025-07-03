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

        public int GetPriceIncrement(GlobalUpgradeType type) {
            switch (type) {
                case GlobalUpgradeType.Power:
                return Power.PriceIncrement;

                case GlobalUpgradeType.InitGold:
                return InitGold.PriceIncrement;
                case GlobalUpgradeType.Hp:
                return HP.PriceIncrement;
                case GlobalUpgradeType.Exp:
                return Exp.PriceIncrement;
            }
            return 0;
        }
        public int GetStartPrice(GlobalUpgradeType type) {
            switch (type) {
                case GlobalUpgradeType.Power:
                return Power.StartPrice;

                case GlobalUpgradeType.InitGold:
                return InitGold.StartPrice;
                case GlobalUpgradeType.Hp:
                return HP.StartPrice;
                case GlobalUpgradeType.Exp:
                return Exp.StartPrice;
            }
            return 0;
        }

        public int GetValueIncrement(GlobalUpgradeType type) {
            switch (type) {
                case GlobalUpgradeType.Power:
                return Power.ValueIncrement;

                case GlobalUpgradeType.InitGold:
                return InitGold.ValueIncrement;
                case GlobalUpgradeType.Hp:
                return HP.ValueIncrement;
                case GlobalUpgradeType.Exp:
                return Exp.ValueIncrement;
            }
            return 0;
        }
    }
}
