using Data;
using Zenject;
using System;
namespace UI
{
    public class MoneyViewModel 
    {
        [Inject]
        private IMoneyRepository _moneyRepo; // model
        public event Action OnDataChanged; // �����Ͱ� ����ɋ� ȣ��� �׼�

        /// <summary>
        /// ������ ���� �˸�
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
