using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Contracts
{
    public interface IGlobalUpgradePurchaseService
    {
        UniTask<bool> TryPurchaseAsync(GlobalUpgradeType type); // 업그레이드 구매 시도
    }
}
