using UnityEngine;

namespace Data
{

    public interface IMoneyRepository {
        long GetValue();
        void SetValue(long value);
    }

    public class MoneyRepository : IMoneyRepository {
        private MoneyModel _money;

        public MoneyRepository() {
            _money = new MoneyModel();
        }

        public long GetValue() {
            return _money.Value;
        }

        public void SetValue(long value) {
            _money.Set(value);
        }
    }
}
