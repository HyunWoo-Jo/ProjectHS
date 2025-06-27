using UnityEngine;
using Zenject;
using Data;
namespace GamePlay
{
    public class TowerPricePolicy : ITowerPricePolicy {
        private int _startPrice = 5;
        [Inject] private TowerPurchaseModel _towerPurchaseModel;

        public void AdvancePrice() { // 타워 구매후 다음 타워 비용 셋팅
            _towerPurchaseModel.towerPriceObservable.Value += 1;
        }

        public int GetCurrentPrice() { // 타워 비용 Get
            return _towerPurchaseModel.towerPriceObservable.Value;
        }

        public int GetStartPrice() { // 시작 비용
            return _startPrice;
        }
    }
}
