using UnityEngine;
using Data;
namespace Contracts
{
    public interface IGlobalUpgradePurchaseService
    {
        bool TryPurchase(UpgradeType type); // ���׷��̵� ���� �õ�
    }
}
