using UnityEngine;
using System.Threading.Tasks;
using System;
namespace Network
{
    /// <summary>
    /// User�� ������ ��������� interface
    /// </summary>
    public interface IUserService
    {
        void GetUserCrystalAsync(Action<int> completeAction);
        void SaveUseCrystalAsync(int userCrystal); 
    }
}
