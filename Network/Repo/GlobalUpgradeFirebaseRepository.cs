using UnityEngine;
using Data;
namespace Network
{

    /// <summary>
    /// ���� Firebase �������� ����
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
