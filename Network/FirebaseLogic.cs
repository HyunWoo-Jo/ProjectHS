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
        /// 로그인 시도 (Email Password 로그인 시도 -> 등록 시도
        /// </summary>
        public async Task GuestLoginAsync() {
            try {
                // Email, Password 로그인 시도
                string systemUID = SystemInfo.deviceUniqueIdentifier;
                string guestEmail = systemUID + "@Guest.com";
                // 로그인 시도
                await _auth.SignInWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // 로그인 실패
                    if (task.IsCanceled || task.IsFaulted) {
                        // 실패시 다음 로직 수행
                    } else { // 성공
                        _user = task.Result.User;
                        return;
                    }
                });
                ///////// 전부 실패시 호출되는 영역 등록 시도
                // 생성 시도
                await _auth.CreateUserWithEmailAndPasswordAsync(guestEmail, systemUID).ContinueWith(task => {
                    // 등록 실패 // 네트워크 오류
                    if (task.IsCanceled || task.IsFaulted) {
                        throw new Exception("Network Err");
                    } else {
                        // 등록 성공
                        _user = task.Result.User;
                        return;
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

        public async Task<bool> IsConnectedAsync() {
            if(_auth.CurrentUser == null) { // 로그인 되어있지 않은 경우
                return false;
            }

            Task<string> task = _user.TokenAsync(true); // 토큰 갱신 요청
            await task;
            if (task.IsFaulted || task.IsCanceled) { // 유효 하지 않은 섹션이면
                return false;
            }

            return true;
        }


        public void Dispose() {
            _user.Dispose();
        }



    }
}
