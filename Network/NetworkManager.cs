using UnityEngine;
using Zenject;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
namespace Network
{
    /// <summary>
    /// Network를 관리하는 클레스
    /// </summary>
    public class NetworkManager : MonoBehaviour, IUserService, IUpgradeService, INetworkService
    {
        [Inject] private INetworkLogic _networkLogic;
        private bool isClosed = false;
        private void Awake() { // 시작시 네트워크 접속 시도
            _networkLogic.Initialize(); // 초기화
            LoginAsync(); // 로그인 시도

        }
        private void OnDestroy() {
            isClosed = true;
        }

        public async void LoginAsync() {
            Debug.Log("try Login");
            try {
                await _networkLogic.GuestLoginAsync();
            } catch  {
                // 로그인 실패 네트워크 문제일 확률이 높음
                await Task.Delay(1000); // 1초 마다 재접속 시도
                if (isClosed) return;
                LoginAsync();
                return;
            }
        }
        public async Task<bool> IsConnectedAsync() {
            return await _networkLogic.IsConnectedAsync();
        }
        




        public int GetUpgradeAsync(string key) {
            throw new NotImplementedException();
        }

        public void SetUpgradeAsync(string key, int value) {
            throw new NotImplementedException();
        }

        ////////////// User Service   
        public void GetUserCrystalAsync(Action<int> completeAction) {
            _networkLogic.GetUserCrystal().ContinueWithOnMainThread(task => {
                if (task.IsFaulted || task.IsCanceled) {
                    return;
                }
                if (task.Result.Exists) {
                    completeAction?.Invoke(Convert.ToInt32(task.Result.Value));
                } else {
                    SaveUseCrystalAsync(0); // 존재하지 않으면 데이터 생성
                }
            });
        }

        public void SaveUseCrystalAsync(int userCrystal) {
            _networkLogic.SetUserCrystal(userCrystal);
        }
        //////////////
    }
}
