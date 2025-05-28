using UnityEngine;

namespace Data
{

    public interface IMoneyRepository {
        long GetValue();
        void SetValue(long value);
    }

  
}
