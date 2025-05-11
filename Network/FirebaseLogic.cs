using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using System;
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
        public async Task GuestLoginAsync() {
            try {
                // Email, Password �α��� �õ�
                string systemUID = SystemInfo.deviceUniqueIdentifier;
                string guestEmail = systemUID + "@Guest.com";
                // �α��� �õ�
                await _auth.SignInWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // �α��� ����
                    if (task.IsCanceled || task.IsFaulted) {
                        // ���н� ���� ���� ����
                    } else { // ����
                        _user = task.Result.User;
                        return;
                    }
                });
                ///////// ���� ���н� ȣ��Ǵ� ���� ��� �õ�
                // ���� �õ�
                await _auth.CreateUserWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // ��� ���� // ��Ʈ��ũ ����
                    if (task.IsCanceled || task.IsFaulted) {
                        throw new Exception("Network Err");
                    } else {
                        // ��� ����
                        _user = task.Result.User;
                        return;
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

        public async Task<bool> IsConnectedAsync() {
            if(_auth.CurrentUser == null) { // �α��� �Ǿ����� ���� ���
                return false;
            }

            Task<string> task = _user.TokenAsync(true); // ��ū ���� ��û
            await task;
            if (task.IsFaulted || task.IsCanceled) { // ��ȿ ���� ���� �����̸�
                return false;
            }

            return true;
        }


        public void Dispose() {
            _user.Dispose();
        }



    }
}
