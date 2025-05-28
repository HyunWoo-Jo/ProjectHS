using UnityEngine;

namespace Data
{
    public interface IGlobalUpgradeRepository : IDataGetterKey<float, UpgradeType>
    {
        // float GetValue(UpgradeType type); IDataGetterKey 정의 되어있음
        void SetValue(UpgradeType type, float value);
    }
}
