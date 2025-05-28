using UnityEngine;

namespace Data
{
    public interface IGlobalUpgradeRepository : IDataGetterKey<float, UpgradeType>
    {
        // float GetValue(UpgradeType type); IDataGetterKey ���� �Ǿ�����
        void SetValue(UpgradeType type, float value);
    }
}
