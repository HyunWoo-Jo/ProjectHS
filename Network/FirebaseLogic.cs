using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using System;
using Firebase.Extensions;
using Cysharp.Threading.Tasks;
namespace Network
{

    public class FirebaseLogic : INetworkLogic {
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private DatabaseReference _databaseReference;
        public void Initialize() {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            _auth = FirebaseAuth.DefaultInstance;
        }

        /// <summary>
        /// 로그인 시도 (Email Password 로그인 시도 -> 등록 시도
        /// </summary>
        public async UniTask GuestLoginAsync() {
            try {
                // Email, Password 로그인 시도
                string systemUID;
                if (PlayerPrefs.HasKey("uid")) {
                    systemUID = PlayerPrefs.GetString("uid");
                } else {
                    systemUID = SystemInfo.deviceUniqueIdentifier;
                    PlayerPrefs.SetString("uid", systemUID);
                }
                string guestEmail = systemUID + "@Guest.com";
                
                // 로그인 시도
                await _auth.SignInWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // 로그인 실패
                    if (task.IsCanceled || task.IsFaulted) {
                        // 실패시 다음 로직 수행
                    } else { // 성공
                        _user = task.Result.User;
                    }
                });
                if (await IsConnectedAsync()) {
                    return;
                }
                /////////// 전부 실패시 호출되는 영역 등록 시도
                // 생성 시도
                await _auth.CreateUserWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // 등록 실패 // 네트워크 오류
                    if (task.IsCanceled || task.IsFaulted) {
                        throw new Exception("Network Err");
                    } else {
                        // 등록 성공
                        _user = task.Result.User;
                    }
                });
            } catch {
                // 네트워크 에러 추정
                throw;
            }
        }

        /// <summary>
        /// 연결 확인
        /// </summary>
        /// <returns></returns>

        public async UniTask<bool> IsConnectedAsync() {
            if(_auth.CurrentUser == null || _user == null) { // 로그인 되어있지 않은 경우
                return false;
            }

            UniTask<string> task = _user.TokenAsync(true).AsUniTask(); // 토큰 갱신 요청
            await task;
            if (task.Status.IsFaulted() || task.Status.IsCanceled()) { // 유효 하지 않은 섹션이면
                return false;
            }

            return true;
        }


        public void Dispose() {
            _user.Dispose();
        }


     

        // UserData
        public UniTask<DataSnapshot> GetUserCrystal() {
            return GetCrystalRef().GetValueAsync().AsUniTask();
        }

        public async UniTask SetUserCrystal(int value) {
            await GetCrystalRef().SetValueAsync(value);
        }

        private DatabaseReference GetCrystalRef() {
            return _databaseReference.Child("UserData").Child(_user.UserId).Child("Crystal");
        }

        // Upgrade
        public UniTask<DataSnapshot> GetAllUpgrade() {
            return GetUpgradeRef().GetValueAsync().AsUniTask();
        }

        public async UniTask SetUpgrade<T>(string key, T value) {
            await GetUpgradeRef().Child(key).SetValueAsync(value);
        }
        private DatabaseReference GetUpgradeRef() {
            return _databaseReference.Child("UserData").Child(_user.UserId).Child("Upgrade");
        }

        public UniTask<DataSnapshot> GetVersion() {
            return _databaseReference.Child("UpgradeTable").Child("Version").GetValueAsync().AsUniTask();
        }
        public UniTask<DataSnapshot> GetUpgradeTable() {
            return _databaseReference.Child("UpgradeTable").GetValueAsync().AsUniTask();
        }

        public UniTask<DataSnapshot> GetUpgradeLevel(string key) {
            return _databaseReference.Child("UserData").Child(_user.UserId).Child("Upgrade").Child(key).GetValueAsync().AsUniTask();
        }
    }
}
