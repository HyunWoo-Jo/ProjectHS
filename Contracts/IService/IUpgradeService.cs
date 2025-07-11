using UnityEngine;

namespace Contracts
{
    public interface IUpgradeService
    {
        void ApplyUpgrade(int index); 
        void Reroll(int index);
    }
}
