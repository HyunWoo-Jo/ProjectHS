using UnityEngine;
using System.Threading.Tasks;
namespace Network
{
    /// <summary>
    /// User의 정보를 가지고오는 interface
    /// </summary>
    public interface IUserService
    {
        Task<long> GetUserMoneyAsync();
        Task SaveUserMoneyAsync(long userMoney); 
    }
}
