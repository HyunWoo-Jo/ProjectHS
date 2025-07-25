using UnityEngine;
using CustomUtility;
using R3;
namespace Data
{
    public class TowerPurchaseModel {
        public ReactiveProperty<int> towerPriceObservable = new (1);
    }
}
