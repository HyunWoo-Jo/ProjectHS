using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Data
{
    public class UpgradeModel : IDataGetterKey<float, UpgradeType>
    {
        private Dictionary<UpgradeType, float> _dataDictionary = new();

        public float GetValue(UpgradeType key) {
            if(_dataDictionary.TryGetValue(key, out var value)) {
                return value;
            }
            return 0;
        }
        public void SetValue(UpgradeType key, float value) {
            _dataDictionary[key] = value;
        }
    
    }
}
