using Data;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public interface IUpgradeSystem
    {
        List<UpgradeDataSO> GetRandomUpgradeDataList(int count);
        bool TryShowRemainUpgradeSelection();
        void ConsumeRemainingCount();
    }
}
