using UnityEngine;
using Zenject;
using Network;
using System.Collections;
using Data;
using Cysharp.Threading.Tasks;
namespace GamePlay
{
    public class MainLobbySceneManager : MonoBehaviour
    {
        [Inject] private NetworkManager _networkManager;
        [Inject] private ICrystalRepository _crystalRepository;
        private bool isClosed = false;
        private void Awake() {
            isClosed = false;
            WaitConnectedNetworkAsync();
        }
        private void OnDestroy() {
            isClosed = true;
        }

        // 네트워크 초기화 까지 대기
        private async void WaitConnectedNetworkAsync() {
            while (true) {
                var result = await _networkManager.IsConnectedAsync();
                if (result) {
                    // 초기화 성공시 load
                    _crystalRepository.LoadValue();
                    return;
                }
                if (isClosed) return;
                await UniTask.Delay(100);
            }
        }

    }
}
