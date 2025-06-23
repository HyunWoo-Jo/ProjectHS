using Cysharp.Threading.Tasks;
using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// ���׷��̵� ������ ������ ���� 
    /// </summary>
    public interface IUpgradeService
    {
        UniTask GetAllUpgradeTableAsync(GlobalUpgradeDataSO tableSO);
        UniTask GetAllUpgradeLevelAsync(Action<Dictionary<string, int>> complate);
        void SetUpgradeAsync<T>(string key, T value);
    }
}
