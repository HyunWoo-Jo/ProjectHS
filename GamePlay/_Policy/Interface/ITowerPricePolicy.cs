using UnityEngine;

namespace GamePlay
{
    public interface ITowerPricePolicy
    {
        int GetCurrentPrice();       // Get 타워 가격
        void AdvancePrice();         // 한 번 구매 후 상승

        int GetStartPrice();
    }
}
