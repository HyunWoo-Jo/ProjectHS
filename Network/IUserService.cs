using UnityEngine;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;
namespace Network
{
    /// <summary>
    /// User의 정보를 가지고오는 interface
    /// </summary>
    public interface IUserService
    {
        UniTask GetUserCrystalAsync(Action<int> completeAction);
        UniTask SaveUseCrystalAsync(int userCrystal);

  
    }
}
