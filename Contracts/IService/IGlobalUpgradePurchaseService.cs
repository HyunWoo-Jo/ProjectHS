using UnityEngine;
using Data;
namespace Contracts
{
    public interface IGlobalUpgradePurchaseService
    {
        bool TryPurchase(UpgradeType type); // 업그레이드 구매 시도
    }
}
