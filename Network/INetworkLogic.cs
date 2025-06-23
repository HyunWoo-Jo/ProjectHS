using UnityEngine;
using System.Threading.Tasks;
using Firebase.Database;
using Cysharp.Threading.Tasks;
namespace Network
{
    public interface INetworkLogic
    {
        
        public void Initialize();
        public void Dispose();

        public UniTask GuestLoginAsync();

        public UniTask<bool> IsConnectedAsync();

        public UniTask<DataSnapshot> GetUserCrystal();
        public UniTask SetUserCrystal(int value);

        public UniTask<DataSnapshot> GetAllUpgrade();
        public UniTask SetUpgrade<T>(string key, T value);

        public UniTask<DataSnapshot> GetVersion();
        public UniTask<DataSnapshot> GetUpgradeTable();
    }
}
