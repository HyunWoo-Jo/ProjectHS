using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contracts {
    public interface IGlobalUpgradeRepository
    {
        UniTask LoadTableAsync();
        UniTask<Dictionary<string, int>> LoadAllUpgradeLevelAsync();

        UniTask<int> GetLevelAsync(GlobalUpgradeType type);
        int GetPrice(GlobalUpgradeType type, int level);
        int GetAbilityValue(GlobalUpgradeType type, int level);
        void SetLevel(GlobalUpgradeType type, int value);

        
    }
}
