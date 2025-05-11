using UnityEngine;

namespace Network
{
    /// <summary>
    /// ���׷��̵� ������ ������ ���� 
    /// </summary>
    public interface IUpgradeService
    {
        int GetUpgradeAsync(string key);
        void SetUpgradeAsync(string key, int value);
    }
}
