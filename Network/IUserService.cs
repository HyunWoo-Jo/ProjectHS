using UnityEngine;
using System.Threading.Tasks;
namespace Network
{
    /// <summary>
    /// User�� ������ ��������� interface
    /// </summary>
    public interface IUserService
    {
        Task<long> GetUserMoneyAsync();
        Task SaveUserMoneyAsync(long userMoney); 
    }
}
