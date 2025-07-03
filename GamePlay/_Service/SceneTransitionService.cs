using UnityEngine;
using Contracts;
using Data;
using Zenject;
using UI;
namespace GamePlay
{
    // 씬 이동을 관리하는 Service
    public class SceneTransitionService : ISceneTransitionService {
        [Inject] private IUIFactory _uiFactory;
        [Inject] private ILoadManager _loadManager;
        private const float _Delay = 0.5f;
        public void LoadScene(SceneName sceneName) {
            IWipeUI wipeUI = _uiFactory.InstanceUI<WipeUI>(100); // UI 생성 요청
            wipeUI.Wipe(WipeDirection.Left, _Delay, false); // 이펙트 호출

            _loadManager.LoadScene(sceneName, _Delay); // 씬이동
        }
    }
}
