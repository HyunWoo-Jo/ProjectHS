using UnityEngine;
using System.Threading.Tasks;
using Firebase.Database;
using Cysharp.Threading.Tasks;
namespace Network
{
    public interface INetworkLogic
    {
        
        void Initialize();
        void Dispose();

        UniTask GuestLoginAsync();

        UniTask<bool> IsConnectedAsync();

        UniTask<DataSnapshot> GetUserCrystal();
        UniTask SetUserCrystal(int value);

        UniTask<DataSnapshot> GetAllUpgrade();
        UniTask<DataSnapshot> GetUpgradeLevel(string key);
        UniTask SetUpgrade<T>(string key, T value);

        UniTask<DataSnapshot> GetVersion();
        UniTask<DataSnapshot> GetUpgradeTable();
    }
}
