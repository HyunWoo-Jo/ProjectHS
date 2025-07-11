using UnityEngine;
using Data;
namespace Contracts
{
    public interface ITowerPurchaseService
    {
        bool TryPurchase(); // 타워 구매 시도
    }
}
