using CustomUtility;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Data
{
    public interface IGlobalUpgradeRepository
    {
        UniTask LoadValue();
        void SetValue(UpgradeType type, int value);
        
        int GetValueLocal(UpgradeType type);
        GlobalUpgradeDataSO GetUpgradeTable();

        void AddChangedHandler(Action handler);
        void RemoveChangedHandler(Action handler);
    }
}
