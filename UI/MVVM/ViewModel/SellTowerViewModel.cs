
using Zenject;
using System;
using Data;
using Contracts;
namespace UI
{
    public class SellTowerViewModel : IInitializable, IDisposable
    {   
        public event Action<int> OnDataChanged; // 데이터가 변경될떄 호출될 액션 (상황에 맞게 변수명을 변경해서 사용)
        [Inject] public TowerSaleModel _model;
        [Inject] private ISellTowerService _sellTowerService;

        public int GetSaleCost() => _model.costObservable.Value;

        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        private void NotifyViewDataChanged(int cost) {
            OnDataChanged?.Invoke(cost);
        }

        // 판매시도
        public void TrySell() {
            _sellTowerService.TrySellTower();
        }
        


        // Zenject가 관리
        public void Initialize() {
            _model.costObservable.OnValueChanged += NotifyViewDataChanged;
        }

        // Zenject가 관리
        public void Dispose() {
            _model.costObservable.OnValueChanged -= NotifyViewDataChanged;
        }
    }
} 
