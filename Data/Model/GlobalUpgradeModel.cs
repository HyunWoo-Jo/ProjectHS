using System.Collections.Generic;
using UnityEngine;
using CustomUtility;
namespace Data
{
    public class GlobalUpgradeModel 
    {
        private Dictionary<string, int> _dataDictionary; // 업그레이드 단계를 저장

        public void SetNewData(Dictionary<string, int> dataDic) {
            _dataDictionary = dataDic;
        }

        public int GetValue(GlobalUpgradeType key) {
            if (_dataDictionary == null) return 0;
            if (_dataDictionary.TryGetValue(key.ToString(), out int value)) {
                return value; 
            }
            return 0;
        }
        public void SetValue(GlobalUpgradeType key, int value) {
            _dataDictionary[key.ToString()] = value;
        }

    }
}
