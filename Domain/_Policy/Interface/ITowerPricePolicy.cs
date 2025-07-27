using UnityEngine;

namespace Domain { 
    public interface ITowerPricePolicy
    {
        int AdvancePrice(int curPrice);         // 한 번 구매 후 상승
    }
}
