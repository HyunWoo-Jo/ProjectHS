using UnityEngine;

namespace Network
{
    public interface IAuthNetworkService
    {
        string GetToken();
        string GetUID();
    }
}
