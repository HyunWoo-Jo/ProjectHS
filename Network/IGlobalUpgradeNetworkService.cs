using Contracts;
using Cysharp.Threading.Tasks;
using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// 업그레이드 정보를 수정하는 서비스
    /// </summary>
    public interface IGlobalUpgradeNetworkService {
     
        UniTask GetAllUpgradeTableAsync(GlobalUpgradeTableSO tableSO);
        UniTask<int> GetUpgradeLevelAsync(string key);
        UniTask<Dictionary<string, int>> GetAllUpgradeLevelAsync();
        void SetUpgradeAsync<T>(string key, T value);
    }
}
