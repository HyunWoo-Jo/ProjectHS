using UnityEngine;

namespace Data
{
    public abstract class UnlockStrategyBaseSO : ScriptableObject
    {
        public abstract bool IsSatisfied(float value);
    }
}
