using CustomUtility;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Data
{
    public interface IGlobalUpgradeRepository
    {
        UniTask LoadValue();
        void SetLevel(GlobalUpgradeType type, int value);
        
        int GetLevelLocal(GlobalUpgradeType type);
        int GetPrice(GlobalUpgradeType type);
        int GetAbilityValue(GlobalUpgradeType type);
       

        void AddChangedListener(Action handler);
        void RemoveChangedListener(Action handler);
    }
}
