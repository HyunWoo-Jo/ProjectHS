using UnityEngine;
using Data;
namespace Contracts
{
    public interface IGlobalUpgradePurchaseService
    {
        bool TryPurchase(GlobalUpgradeType type); // ���׷��̵� ���� �õ�
    }
}
