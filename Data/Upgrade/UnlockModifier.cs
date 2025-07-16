using System;
using UnityEngine;

namespace Data
{

    [Serializable]
    public class UnlockModifier
    {
        public float value;
        [SerializeField] private UnlockStrategyBaseSO _unlockStrategy;

        public bool IsSatisfied() {
            return _unlockStrategy.IsSatisfied(value);
        }
    }
}
