using UnityEngine;
using Contracts;
using Data;
using Zenject;
using UI;
namespace GamePlay
{
    // �� �̵��� �����ϴ� Service
    public class SceneTransitionService : ISceneTransitionService {
        [Inject] private IUIFactory _uiFactory;
        [Inject] private ILoadManager _loadManager;
        private const float _Delay = 0.5f;
        public void LoadScene(SceneName sceneName) {
            IWipeUI wipeUI = _uiFactory.InstanceUI<WipeUI>(100); // UI ���� ��û
            wipeUI.Wipe(WipeDirection.Left, _Delay, false); // ����Ʈ ȣ��

            _loadManager.LoadScene(sceneName, _Delay); // ���̵�
        }
    }
}
