using UnityEngine;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;
namespace Network
{
    /// <summary>
    /// User의 정보를 가지고오는 interface
    /// </summary>
    public interface IUserNetworkService
    {
        UniTask<int> GetUserCrystalAsync();
        UniTask SaveUseCrystalAsync(int userCrystal);

  
    }
}
