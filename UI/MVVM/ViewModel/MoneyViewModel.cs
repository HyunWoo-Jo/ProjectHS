using Data;
using Zenject;
using System;
namespace UI
{
    public class MoneyViewModel 
    {
        [Inject]
        private IMoneyRepository _moneyRepo; // model
        public event Action OnDataChanged; // 데이터가 변경될떄 호출될 액션

        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        private void NotifyViewDataChanged() {
            OnDataChanged?.Invoke();
        }

        public long GetMoney => _moneyRepo.GetValue();

        public void SetData(long value) {
            _moneyRepo.SetValue(value);
            NotifyViewDataChanged();
        }
    }
} 
