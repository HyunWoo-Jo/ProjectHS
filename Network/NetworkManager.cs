using UnityEngine;
using Zenject;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Collections;
using Data;
using Cysharp.Threading.Tasks;
using Firebase.Database;
namespace Network
{
    /// <summary>
    /// Network�� �����ϴ� Ŭ����
    /// </summary>
    public class NetworkManager : MonoBehaviour, IUserNetworkService, IGlobalUpgradeNetworkService, INetworkService
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
        public async UniTask<bool> IsConnectedAsync() {
            return await _networkLogic.IsConnectedAsync();
        }
        






        ////////////// User Service   
        public async UniTask GetUserCrystalAsync(Action<int> completeAction) {
            await _networkLogic.GetUserCrystal().ContinueWith(task => {
                if (task.Exists) {
                    completeAction?.Invoke(Convert.ToInt32(task.Value));
                }
            });
           
        }

        public async UniTask SaveUseCrystalAsync(int userCrystal) {
            await _networkLogic.SetUserCrystal(userCrystal);
        }

        ///////// Upgrade Service
        
        /// <summary>
        /// ScritableObject Upgrade Table�� ������
        /// </summary>
        public UniTask GetAllUpgradeTableAsync(GlobalUpgradeDataSO tableSO) {
            // ���� üũ
            return _networkLogic.GetVersion().ContinueWith(task => {
                if (task.Exists) {        
                    // ������ �ٸ� ��츸 table�� �о�� (���� ���� ������ �߰�)
                    if ((string)task.Value != tableSO.Version) {
                        _networkLogic.GetUpgradeTable().ContinueWith(task => {
                            if (task.Exists) {
                                JsonUtility.FromJsonOverwrite(task.GetRawJsonValue(), tableSO);
                            }
                        });
                    }
                }
            });
        }
        public UniTask GetAllUpgradeLevelAsync(Action<Dictionary<string, int>> complate) {

            return _networkLogic.GetAllUpgrade().ContinueWith(task => {
                if (task.Exists) {
                    var objDict = task.Value as IDictionary<string, object>;
                    if (objDict == null) {
                        Debug.LogError("��ȯ ����");
                        return;
                    }
                    var intDict = new Dictionary<string, int>();
                    foreach (var kvp in objDict) {
                        try {
                            intDict[kvp.Key] = Convert.ToInt32(kvp.Value); // long to int32�� ��ȯ (Firebase�� �⺻ long��)
                        } catch (Exception e) {
                            Debug.LogWarning($"Ű {kvp.Key} ��ȯ ����: {e.Message}");
                        }
                    }
                    complate?.Invoke(intDict);
                }
            });
        }

        public void SetUpgradeAsync<T>(string key, T value) {
            _networkLogic.SetUpgrade(key, value);
        }

        //////////////
    }
}
