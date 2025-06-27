using UnityEngine;
using Zenject;
using Data;
using Contracts;
namespace GamePlay
{
    // Ÿ�� ���Ÿ� �ൿ
    public class TowerPurchaseService : ITowerPurchaseService {
        [Inject] private GoldModel _gold;
        [Inject] private TowerSystem _towerSystem; // Ÿ�� �ý���
        [Inject] private ITowerPricePolicy _pricePolicy; // ���� ��å

        public bool TryPurchase() { // Ÿ�� ���� �õ�
            int cost = _pricePolicy.GetCurrentPrice();
            if (_gold.goldObservable.Value < cost) return false;
            _gold.goldObservable.Value -= cost;
            _towerSystem.AddTower();
            _pricePolicy.AdvancePrice();
            return true;
        }
    }
}
