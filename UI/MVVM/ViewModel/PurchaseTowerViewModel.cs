
using Zenject;
using System;
using Data;
namespace UI
{
    public class PurchaseTowerViewModel : IInitializable, IDisposable
    {
        [Inject] private PurchaseTowerModel _model;
        public event Action OnButtonClick;
        public event Action<int> OnDataChanged;

        /// <summary>
        /// 버튼 클릭
        /// </summary>
        public void ButtonClick() {
            OnButtonClick?.Invoke();
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
        }
        public void Dispose() {
            _model.towerPriceObservable.OnValueChanged -= NotifyDataChanged;
        }


    }
} 
