
using Zenject;
using System;
using Data;
using Contracts;
namespace UI
{
    public class TowerPurchaseViewModel : IInitializable, IDisposable
    {
        [Inject] private TowerPurchaseModel _model;
        [Inject] private ITowerPurchaseService _towerPurchaseService;
        public event Func<bool> OnPurchaseButtonClick;
        public event Action<int> OnDataChanged;

        /// <summary>
        /// 버튼 클릭
        /// </summary>
        public bool PurchaseButtonClick() {
            return OnPurchaseButtonClick?.Invoke() ?? false; // 실패시 false
        }

        public void Update() {
            NotifyDataChanged(_model.towerPriceObservable.Value);
        }

        private void NotifyDataChanged(int value) {
            OnDataChanged?.Invoke(value);
        }


        // Jenject에서 관리
        public void Initialize() {
            _model.towerPriceObservable.OnValueChanged += NotifyDataChanged;
            OnPurchaseButtonClick += _towerPurchaseService.TryPurchase;
        }
        public void Dispose() {
            _model.towerPriceObservable.OnValueChanged -= NotifyDataChanged;
            OnPurchaseButtonClick -= _towerPurchaseService.TryPurchase;
        }


    }
} 
