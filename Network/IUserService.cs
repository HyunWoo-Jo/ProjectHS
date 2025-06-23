using UnityEngine;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;
namespace Network
{
    /// <summary>
    /// User�� ������ ��������� interface
    /// </summary>
    public interface IUserService
    {
        UniTask GetUserCrystalAsync(Action<int> completeAction);
        UniTask SaveUseCrystalAsync(int userCrystal);

  
    }
}
