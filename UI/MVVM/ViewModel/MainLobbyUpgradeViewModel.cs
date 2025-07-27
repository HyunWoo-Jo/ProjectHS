
using Zenject;
using System;
using Data;
using System.Diagnostics;
using CustomUtility;
using Contracts;
using Domain;
using R3;
namespace UI
{
    public class MainLobbyUpgradeViewModel {
        [Inject] private GlobalUpgradeModel _model;
        [Inject] private IGlobalUpgradePurchaseService _purchaseService;

        public ReadOnlyReactiveProperty<int> GetRO_UpgradeData(GlobalUpgradeType type) => _model.GetRO_UpgradeData(type);

        public int GetPrice(GlobalUpgradeType type) => _model.GetPrice(type);

        public int GetLevel(GlobalUpgradeType type) => _model.GetLevel(type);
        public int GetAbilityValue(GlobalUpgradeType type) => _model.GetAbilityPower(type);

        public void TryPurchase(GlobalUpgradeType type) {
            _purchaseService.TryPurchaseAsync(type);
        }

    }
} 
