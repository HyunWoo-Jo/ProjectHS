using UnityEngine;
using System.Collections.Generic; 
namespace Data
{
    [CreateAssetMenu(fileName = "UpgradeDataSO", menuName = "Scriptable Objects/UpgradeDataSO")]
    public class UpgradeDataSO : ScriptableObject
    {
        public UpgradeType upgradeType;
        [SerializeField] private List<UnlockStrategyBaseSO> _unlockStrategyBaseList = new();

    }
}
