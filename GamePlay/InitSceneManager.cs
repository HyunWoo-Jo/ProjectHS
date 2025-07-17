using UnityEngine;
using Network;
using Data;
using Zenject;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
namespace GamePlay
{
    public class InitSceneManager : MonoBehaviour
    {
        [Inject] private NetworkManager _networkManager; // Network 
        [Inject] private ICrystalRepository _crystalRepository; // 크리스탈 repo
        [Inject] private IGlobalUpgradeRepository _globalUpgradeRepository; // 업그레이드 repo
        [SerializeField] private InitSceneUI _initSceneUI;
        private bool isClosed = false;
        private void Awake() {
#if UNITY_EDITOR // 검증
            Assert.IsNotNull(_initSceneUI);
#endif

            isClosed = false;
            InitLogicAsync();

        }
        private void OnDestroy() {
            isClosed = true;
        }

        // 네트워크 초기화 까지 대기
        private async void InitLogicAsync() {
            
            // 연결 확인
            await CheckCnnectedLoop();
            // 지연
            await UniTask.Delay(500);
            // UpgradeTable 읽기
            await GetGlobalUgradeLoop();
            // UserData 읽기
            await GetUserDataLoop();
            // 메인 로비로 이동
            await SceneManager.LoadSceneAsync("MainLobbyScene");
            
        }
        private async UniTask CheckCnnectedLoop() {
            // Login 시도
            _initSceneUI.UpdateTextFromThread("Check Cnnected Network");
            while (true) {
                var result = await _networkManager.IsConnectedAsync();
                if (result) {
                    
                    break;
                }
                if (isClosed) break;
                await UniTask.Delay(100);
            }
        }
        private async UniTask GetGlobalUgradeLoop() {
            _initSceneUI.UpdateTextFromThread("Update Table");
            // upgradeTable, Level을 로드
            await _globalUpgradeRepository.LoadValue();
        }
        private async UniTask GetUserDataLoop() {
            _initSceneUI.UpdateTextFromThread("Update User Data");
            await _crystalRepository.LoadValue();
        }
    }
}
