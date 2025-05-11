using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// Network 핵심 로직을 가지고 오는 interface
    /// </summary>
    public interface INetworkService
    {
        void LoginAsync();

        Task<bool> IsConnectedAsync();
    }
}
