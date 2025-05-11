using UnityEngine;
using System.Threading.Tasks;
namespace Network
{
    public interface INetworkLogic
    {
        public void Initialize();
        public Task GuestLoginAsync();

        public Task<bool> IsConnectedAsync();

        public void Dispose();
    }
}
