using UnityEngine;
using Zenject;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
namespace Network
{
    /// <summary>
    /// Network�� �����ϴ� Ŭ����
    /// </summary>
    public class NetworkManager : MonoBehaviour, IUserService, IUpgradeService, INetworkService
    {
        [Inject] private INetworkLogic _networkLogic;
        private bool isClosed = false;
        private void Awake() { // ���۽� ��Ʈ��ũ ���� �õ�
            _networkLogic.Initialize(); // �ʱ�ȭ
            LoginAsync(); // �α��� �õ�

        }
        private void OnDestroy() {
            isClosed = true;
        }

        public async void LoginAsync() {
            Debug.Log("try Login");
            try {
                await _networkLogic.GuestLoginAsync();
            } catch  {
                // �α��� ���� ��Ʈ��ũ ������ Ȯ���� ����
                await Task.Delay(1000); // 1�� ���� ������ �õ�
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
                    SaveUseCrystalAsync(0); // �������� ������ ������ ����
                }
            });
        }

        public void SaveUseCrystalAsync(int userCrystal) {
            _networkLogic.SetUserCrystal(userCrystal);
        }
        //////////////
    }
}
