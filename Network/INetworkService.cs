using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// Network �ٽ� ������ ������ ���� interface
    /// </summary>
    public interface INetworkService
    {
        void LoginAsync();

        Task<bool> IsConnectedAsync();
    }
}
