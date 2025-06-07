using UnityEngine;

namespace Data
{
    public class SlotData
    {
        public SlotState slotState;
        private TowerData _towerData; // null 이면 비어있음

        public void SetTowerData(TowerData towerData) {
            this._towerData = towerData;
        }
        public bool GetTowerData(out TowerData towerData) {
            if (IsUsed()) {
                towerData = _towerData;
                return true;
            }
            towerData = null;
            return false;

        }

        public bool IsUsed() {
            if(_towerData != null) {
                return true;
            }
            return false;
        }
    }
}
