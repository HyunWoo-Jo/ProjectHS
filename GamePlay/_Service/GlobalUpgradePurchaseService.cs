using UnityEngine;
using Contracts;
using Network;
using Zenject;
using Data;
namespace GamePlay
{
    public class GlobalUpgradePurchaseService : IGlobalUpgradePurchaseService 
    {
        [Inject] private IGlobalUpgradeRepository _globalUpgradeRepo;
        [Inject] private ICrystalRepository _crystalRepo;
        public bool TryPurchase(GlobalUpgradeType type) {
            int price = _globalUpgradeRepo.GetPrice(type);
            int curCristal = _crystalRepo.GetValue();
            if (price <= curCristal && _crystalRepo.TrySpend(price)) {
                int level = _globalUpgradeRepo.GetLevelLocal(type);
                _globalUpgradeRepo.SetLevel(type, level + 1);
                return true;
            }
            return false;     
        }
    }
}
