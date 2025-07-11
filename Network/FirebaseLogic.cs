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
        /// �α��� �õ� (Email Password �α��� �õ� -> ��� �õ�
        /// </summary>
        public async UniTask GuestLoginAsync() {
            try {
                // Email, Password �α��� �õ�
                string systemUID;
                if (PlayerPrefs.HasKey("uid")) {
                    systemUID = PlayerPrefs.GetString("uid");
                } else {
                    systemUID = SystemInfo.deviceUniqueIdentifier;
                    PlayerPrefs.SetString("uid", systemUID);
                }
                string guestEmail = systemUID + "@Guest.com";
                
                // �α��� �õ�
                await _auth.SignInWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // �α��� ����
                    if (task.IsCanceled || task.IsFaulted) {
                        // ���н� ���� ���� ����
                    } else { // ����
                        _user = task.Result.User;
                    }
                });
                if (await IsConnectedAsync()) {
                    return;
                }
                /////////// ���� ���н� ȣ��Ǵ� ���� ��� �õ�
                // ���� �õ�
                await _auth.CreateUserWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // ��� ���� // ��Ʈ��ũ ����
                    if (task.IsCanceled || task.IsFaulted) {
                        throw new Exception("Network Err");
                    } else {
                        // ��� ����
                        _user = task.Result.User;
                    }
                });
            } catch {
                // ��Ʈ��ũ ���� ����
                throw;
            }
        }

        /// <summary>
        /// ���� Ȯ��
        /// </summary>
        /// <returns></returns>

        public async UniTask<bool> IsConnectedAsync() {
            if(_auth.CurrentUser == null || _user == null) { // �α��� �Ǿ����� ���� ���
                return false;
            }

            UniTask<string> task = _user.TokenAsync(true).AsUniTask(); // ��ū ���� ��û
            await task;
            if (task.Status.IsFaulted() || task.Status.IsCanceled()) { // ��ȿ ���� ���� �����̸�
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
    }
}
