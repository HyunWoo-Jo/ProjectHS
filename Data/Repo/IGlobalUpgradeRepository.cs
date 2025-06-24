using CustomUtility;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Data
{
    public interface IGlobalUpgradeRepository
    {
        UniTask LoadValue();
        void SetLevel(UpgradeType type, int value);
        
        int GetLevelLocal(UpgradeType type);
        int GetPrice(UpgradeType type);
        int GetAbilityValue(UpgradeType type);
       

        void AddChangedHandler(Action handler);
        void RemoveChangedHandler(Action handler);
    }
}
