using UnityEngine;
using System.Threading.Tasks;
using System;
namespace Network
{
    /// <summary>
    /// User의 정보를 가지고오는 interface
    /// </summary>
    public interface IUserService
    {
        void GetUserCrystalAsync(Action<int> completeAction);
        void SaveUseCrystalAsync(int userCrystal); 
    }
}
