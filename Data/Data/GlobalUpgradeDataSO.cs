using UnityEngine;
using CustomUtility;
using UnityEditor.Graphs;
namespace Data
{
    
    [CreateAssetMenu(fileName = "GlobalUpgradeDataSO", menuName = "Scriptable Objects/GlobalUpgradeDataSO")]
    public class GlobalUpgradeDataSO : ScriptableObject
    {
        [ReadEditor] public string Version;
        // Hp
        [ReadEditor] public int HpStartPrice; // 시작 비용
        [ReadEditor] public int HpPriceIncrement; // 단계에 따른 가격 증가량
        [ReadEditor] public int HpValueIncrement; // 단계에 따른 수치증가량
        

        private int _hash;

        public int GetPrice(UpgradeType type) {
            switch (type) {
                case UpgradeType.Power:
                return HpPriceIncrement;

                case UpgradeType.InitGold:
                break;
                case UpgradeType.Hp:
                break;
                case UpgradeType.ExpPercent:
                break;
            }
            return 0;
        }
        public int GetStartPrice(UpgradeType type) {
            switch (type) {
                case UpgradeType.Power:
                return HpStartPrice;

                case UpgradeType.InitGold:
                break;
                case UpgradeType.Hp:
                break;
                case UpgradeType.ExpPercent:
                break;
            }
            return 0;
        }
    }
}
