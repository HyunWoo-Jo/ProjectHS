using UnityEngine;

namespace Data
{
    public class SlotData
    {
        public SlotState slotState;
        private TowerData _towerData; // null �̸� �������

        public void SetTowerData(TowerData towerData) {
            this._towerData = towerData;
        }
        public bool TryGetTowerData(out TowerData towerData) {
            if (IsUsed()) {
                towerData = _towerData;
                return true;
            }
            towerData = null;
            return false;
        }
        public TowerData GetTowerData() {
            return _towerData;
        }

        public bool IsUsed() {
            if(_towerData != null) {
                return true;
            }
            return false;
        }
    }
}
