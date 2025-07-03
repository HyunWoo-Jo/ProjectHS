using UnityEngine;
using CustomUtility;
using UnityEditor.Graphs;
using System;
namespace Data
{
    [Serializable]
    public class GlobalUpgradeData {
        [ReadEditor] public int StartPrice; // ���� ���
        [ReadEditor] public int PriceIncrement; // �ܰ迡 ���� ���� ������
        [ReadEditor] public int ValueIncrement; // �ܰ迡 ���� ��ġ������
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
