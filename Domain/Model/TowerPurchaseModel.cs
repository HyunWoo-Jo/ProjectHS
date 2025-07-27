using UnityEngine;
using CustomUtility;
using R3;
using Zenject;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
namespace Domain
{
    public class TowerPurchaseModel {
        [Inject] private ITowerPricePolicy _towerPricePolicy;

        private ReactiveProperty<int> _towerPriceObservable = new (1);
        public ReadOnlyReactiveProperty<int> RO_TowerPriceObservable => _towerPriceObservable;

        public int TowerPrice {
            get { return _towerPriceObservable.Value; }
            private set { _towerPriceObservable.Value = value; }
        }

        public void AddvanceTowerPrice() {
            _towerPriceObservable.Value = _towerPricePolicy.AdvancePrice(TowerPrice);
        }
        public void AddTowerPrice(int value) {
            if (value < 0) return;
            _towerPriceObservable.Value += value;
        }
        public void SetTowerPrice(int value) {
            if (value < 0) return;
            _towerPriceObservable.Value = value;

        }

        public void Notify() => _towerPriceObservable.ForceNotify();

    }
}
