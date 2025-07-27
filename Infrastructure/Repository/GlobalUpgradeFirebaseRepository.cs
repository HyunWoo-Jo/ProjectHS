using UnityEngine;
using Zenject;
using System;
using CustomUtility;
using Cysharp.Threading.Tasks;
using Contracts;
using Data;
using Network;
using System.Collections.Generic;
namespace Infrastructure {

    /// <summary>
    /// 이후 Firebase 로직으로 변경
    /// </summary>
    public class GlobalUpgradeFirebaseRepository : IGlobalUpgradeRepository
    {
        [Inject] private IGlobalUpgradeNetworkService _upgradeService;
        [Inject] private GlobalUpgradeTableSO _tableSO;



        public UniTask<Dictionary<string, int>> LoadAllUpgradeLevelAsync() => _upgradeService.GetAllUpgradeLevelAsync();

        public UniTask LoadTableAsync() {
            return _upgradeService.GetAllUpgradeTableAsync(_tableSO);
        }

        /// Value
        public int GetPrice(GlobalUpgradeType type, int level) {
            return level * _tableSO.GetPriceIncrement(type) + _tableSO.GetStartPrice(type); 
        }
        public int GetAbilityValue(GlobalUpgradeType type, int level) {
            return level * _tableSO.GetValueIncrement(type);
        }


        //// Level
        public void SetLevel(GlobalUpgradeType type, int value) {
            _upgradeService.SetUpgradeAsync(type.ToString(), value); // 네트워크 업데이트 요청
        }

        public UniTask<int> GetLevelAsync(GlobalUpgradeType type) {
           return _upgradeService.GetUpgradeLevelAsync(type.ToString());
        }
    }
}
