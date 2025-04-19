using UnityEngine;
using Data;
using Zenject;
using System;
namespace UI
{
    public class MoneyViewModel
    {
        [Inject]
        private IMoneyRepository _moneyRepo; // model

        public event Action<long> OnDataChanged; // 데이터 변경

        public void SetData(long value) {
            _moneyRepo.SetValue(value);
            NotifyViewDataChanged();
        }

        private void NotifyViewDataChanged() {
            OnDataChanged?.Invoke(_moneyRepo.GetValue());
        }

    }
}
