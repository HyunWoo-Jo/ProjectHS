using UnityEngine;
using CustomUtility;
using R3;
namespace Domain
{
    public sealed class TowerSaleModel {
        private ReactiveProperty<int> _costObservable = new(0);

        public ReadOnlyReactiveProperty<int> RO_TowerCostObservable => _costObservable;

        public int TowerCost {
            get { return _costObservable.Value; }
            private set { _costObservable.Value = value; }
        }

        public void SetTowerCost(int cost) {
            if (cost < 0) return;
            _costObservable.Value = cost;
        }

        public void Notify() => _costObservable.ForceNotify();
    }
}
