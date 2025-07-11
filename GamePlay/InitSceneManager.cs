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
        [Inject] private ICrystalRepository _crystalRepository; // ũ����Ż repo
        [Inject] private IGlobalUpgradeRepository _globalUpgradeRepository; // ���׷��̵� repo
        [SerializeField] private InitSceneUI _initSceneUI;
        private bool isClosed = false;
        private void Awake() {
#if UNITY_EDITOR // ����
            Assert.IsNotNull(_initSceneUI);
#endif

            isClosed = false;
            InitLogicAsync();

        }
        private void OnDestroy() {
            isClosed = true;
        }

        // ��Ʈ��ũ �ʱ�ȭ ���� ���
        private async void InitLogicAsync() {
            // ���� Ȯ��
            await CheckCnnectedLoop();
            // UpgradeTable �б�
            await GetUgradeTableLoop();
            // UserData �б�
            await GetUserDataLoop();
            // ���� �κ�� �̵�
            await SceneManager.LoadSceneAsync("MainLobbyScene");
            
        }
        private async UniTask CheckCnnectedLoop() {
            // Login �õ�
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
        private async UniTask GetUgradeTableLoop() {
            // upgradeTable �ε�
            _initSceneUI.UpdateTextFromThread("Update Table");
            // ���ο��� ���� üũ�� ����(�ٸ���쿡�� ������Ʈ)
            await _globalUpgradeRepository.LoadValue();
        }
        private async UniTask GetUserDataLoop() {
            _initSceneUI.UpdateTextFromThread("Update User Data");
            await _crystalRepository.LoadValue();
        }
    }
}
