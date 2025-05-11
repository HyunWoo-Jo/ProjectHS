using UnityEngine;

namespace Network
{
    public interface IAuthService
    {
        string GetToken();
        string GetUID();
    }
}
