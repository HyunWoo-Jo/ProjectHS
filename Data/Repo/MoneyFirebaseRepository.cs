using UnityEngine;

namespace Data
{
    public class MoneyFirebaseRepository : IMoneyRepository {
        private MoneyModel _money;

        public MoneyFirebaseRepository() {
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
