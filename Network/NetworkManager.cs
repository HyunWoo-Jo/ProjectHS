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
    /// Network를 관리하는 클레스
    /// </summary>
    public class NetworkManager : MonoBehaviour, IUserNetworkService, IGlobalUpgradeNetworkService, INetworkService
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
        /// ScritableObject Upgrade Table을 갱신함
        /// </summary>
        public UniTask GetAllUpgradeTableAsync(GlobalUpgradeDataSO tableSO) {
            // 버전 체크
            return _networkLogic.GetVersion().ContinueWith(task => {
                if (task.Exists) {        
                    // 버전이 다를 경우만 table을 읽어옴 (추후 검증 로직도 추가)
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
                        Debug.LogError("변환 실패");
                        return;
                    }
                    var intDict = new Dictionary<string, int>();
                    foreach (var kvp in objDict) {
                        try {
                            intDict[kvp.Key] = Convert.ToInt32(kvp.Value); // long to int32로 변환 (Firebase는 기본 long형)
                        } catch (Exception e) {
                            Debug.LogWarning($"키 {kvp.Key} 변환 실패: {e.Message}");
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
