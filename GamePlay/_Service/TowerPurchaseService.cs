using UnityEngine;
using Zenject;
using Contracts;
using Domain;
using Data;
namespace GamePlay
{
    // 타워 구매를 행동
    public class TowerPurchaseService : ITowerPurchaseService {
        [Inject] private GoldModel _goldModel;
        [Inject] private TowerPurchaseModel _towerPurchaseModel;
        [Inject] private ITowerSystem _towerSystem;
        /// <summary>
        /// 구매 시도
        /// </summary>
        /// <returns></returns>
        public bool TryPurchase() {
            int index = _towerSystem.SerchEmptySlot();
            if(index != -1 && _goldModel.TrySpendGold(_towerPurchaseModel.TowerPrice)) {
                _towerSystem.AddTower(index);
                _towerPurchaseModel.AddvanceTowerPrice();
                return true;
            }
            return false;
        }
    }
}
