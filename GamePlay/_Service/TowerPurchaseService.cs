using UnityEngine;
using Zenject;
using Data;
using Contracts;
namespace GamePlay
{
    // 타워 구매를 행동
    public class TowerPurchaseService : ITowerPurchaseService {
        [Inject] private GoldModel _gold;
        [Inject] private ITowerSystem _towerSystem; // 타워 시스템
        [Inject] private ITowerPricePolicy _pricePolicy; // 가격 정책

        public bool TryPurchase() { // 타워 구매 시도
            int cost = _pricePolicy.GetCurrentPrice();
            if (_gold.goldObservable.Value < cost) return false;
            if (_towerSystem.TryAddTower()) { // 구매 성공시 
                _gold.goldObservable.Value -= cost;
                _pricePolicy.AdvancePrice();
                return true;
            }
            
            return false;
        }
    }
}
