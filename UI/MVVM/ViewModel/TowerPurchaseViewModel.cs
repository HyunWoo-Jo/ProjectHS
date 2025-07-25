
using Zenject;
using System;
using Data;
using Contracts;
using R3;
namespace UI
{
    public class TowerPurchaseViewModel 
    {
        [Inject] private TowerPurchaseModel _model;
        [Inject] private ITowerPurchaseService _towerPurchaseService;
        public event Func<bool> OnPurchaseButtonClick;


        public ReadOnlyReactiveProperty<int> RO_TowerPriceObservable => _model.towerPriceObservable;

        /// <summary>
        /// 버튼 클릭
        /// </summary>
        public bool PurchaseButtonClick() {
            return OnPurchaseButtonClick?.Invoke() ?? false; // 실패시 false
        }


        /// <summary>
        /// 갱신
        /// </summary>
        public void Notify() {
            _model.towerPriceObservable.ForceNotify();
        }





    }
} 
