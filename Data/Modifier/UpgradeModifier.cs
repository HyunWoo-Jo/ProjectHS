using System;
using UnityEngine;
using Contracts;
namespace Data {
    [Serializable]
    public sealed class UpgradeModifier
    {
        public float value;
        [SerializeField] private UpgradeStrategyBaseSO _upgradeStrategy;

        public void Apply() {
            _upgradeStrategy.Apply(value);
        }
    }
}
