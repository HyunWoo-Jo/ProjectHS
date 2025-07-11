using UnityEngine;
using Zenject;
using Data;
namespace GamePlay
{
    public class TowerPricePolicy : ITowerPricePolicy {
        private int _startPrice = 5;
        [Inject] private TowerPurchaseModel _towerPurchaseModel;

        public void AdvancePrice() { // Ÿ�� ������ ���� Ÿ�� ��� ����
            _towerPurchaseModel.towerPriceObservable.Value += 1;
        }

        public int GetCurrentPrice() { // Ÿ�� ��� Get
            return _towerPurchaseModel.towerPriceObservable.Value;
        }

        public int GetStartPrice() { // ���� ���
            return _startPrice;
        }
    }
}
