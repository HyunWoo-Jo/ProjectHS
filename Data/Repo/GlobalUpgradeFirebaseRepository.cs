using UnityEngine;

namespace Data
{

    /// <summary>
    /// 이후 Firebase 로직으로 변경
    /// </summary>
    public class GlobalUpgradeFirebaseRepository : IGlobalUpgradeRepository
    {
        private UpgradeModel _upgradeModel = new();


        public float GetValue(UpgradeType key) {
           return _upgradeModel.GetValue(key);
        }

        public void SetValue(UpgradeType type, float value) {
            _upgradeModel.SetValue(type, value);
        }
    }
}
