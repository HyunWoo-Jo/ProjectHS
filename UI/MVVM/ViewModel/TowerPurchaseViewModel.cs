
using Zenject;
using System;
using Data;
using Contracts;
using R3;
using Domain;
namespace UI
{
    public class TowerPurchaseViewModel 
    {
        [Inject] private TowerPurchaseModel _model;
        [Inject] private ITowerPurchaseService _towerPurchaseService;



        public ReadOnlyReactiveProperty<int> RO_TowerPriceObservable => _model.RO_TowerPriceObservable;

        public bool TryPurchase() {
            return _towerPurchaseService.TryPurchase();
        }

        /// <summary>
        /// 갱신
        /// </summary>
        public void Notify() => _model.Notify();

    }
} 
