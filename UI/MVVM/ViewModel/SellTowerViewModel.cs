
using Zenject;
using System;
using Data;
using Contracts;
using R3;
namespace UI
{
    public class SellTowerViewModel
    {   
        [Inject] public TowerSaleModel _model;
        [Inject] private ISellTowerService _sellTowerService;

        public ReadOnlyReactiveProperty<int> RO_CostObservable => _model.costObservable;


        /// <summary>
        /// 갱신
        /// </summary>
        public void Notify() {
            _model.costObservable.ForceNotify();
        }

        // 판매시도
        public void TrySell() {
            _sellTowerService.TrySellTower();
        }
        


    }
} 
