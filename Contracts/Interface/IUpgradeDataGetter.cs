using UnityEngine;
namespace Contracts
{
    public interface IUpgradeData : IApply, IUnlock
    {
     
        int Rarity();
        Sprite Sprite();
        string UpgradeName();
        string Description();
    }
}
