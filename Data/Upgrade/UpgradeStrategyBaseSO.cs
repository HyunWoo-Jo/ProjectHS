using UnityEngine;

namespace Data
{
 
    public abstract class UpgradeStrategyBaseSO : ScriptableObject
    {
        public abstract void Apply(float value);
    }
}
