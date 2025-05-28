using UnityEngine;

namespace GamePlay
{
    public interface IStatUpgradeService : IUpgradeService
    {
        void UpgradeTowerSpeed(float value);

        void UpgradeTowerPower(float value);
    }
}
