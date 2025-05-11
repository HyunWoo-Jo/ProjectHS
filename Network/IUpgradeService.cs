using UnityEngine;

namespace Network
{
    /// <summary>
    /// 업그레이드 정보를 가지고 오는 
    /// </summary>
    public interface IUpgradeService
    {
        int GetUpgradeAsync(string key);
        void SetUpgradeAsync(string key, int value);
    }
}
