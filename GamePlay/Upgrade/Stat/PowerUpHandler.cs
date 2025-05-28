using UnityEngine;
using Zenject;
using Data;
namespace GamePlay
{
    public class PowerUpHandler : IUpgradeCommandHandler {

        [Inject] private IStatUpgradeService _upgradeService;

       
        public void Execute(UpgradePayload payload) {
            
            _upgradeService.UpgradeTowerPower(payload.value.Value);
        }
    }
}
