using UnityEngine;
using System.Threading.Tasks;
using Firebase.Database;
namespace Network
{
    public interface INetworkLogic
    {
        
        public void Initialize();
        public void Dispose();

        public Task GuestLoginAsync();

        public Task<bool> IsConnectedAsync();

        public Task<DataSnapshot> GetUserCrystal();
        public void SetUserCrystal(int value);
    }
}
