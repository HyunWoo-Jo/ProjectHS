using UnityEngine;
using Contracts;
using Network;
using Zenject;
using Data;
using Cysharp.Threading.Tasks;
using Domain;
namespace GamePlay
{
    public class GlobalUpgradePurchaseService : IGlobalUpgradePurchaseService {
        [Inject] private GlobalUpgradeModel _globalUpgradModel;
        [Inject] private CrystalModel _crystalModel;
        public async UniTask<bool> TryPurchaseAsync(GlobalUpgradeType type) {

            var levelTask = _globalUpgradModel.GetLevelAsync(type);
            var crystalTask = _crystalModel.LoadData();
            int level = await levelTask;
            int crystal = await crystalTask;

            int price = _globalUpgradModel.GetPrice(type, level);

            if (price <= crystal && await _crystalModel.TrySpend(price)) {
                _globalUpgradModel.SetLevel(type, level + 1);
                return true;
            }
            return false;
        }

    }
}
