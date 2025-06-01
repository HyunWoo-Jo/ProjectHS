
using Data;
using System.Diagnostics;
using System;
using Zenject;

namespace UI
{
    // 메인로비의 Navigate UI 를 관리
    public class MainLobbyNavigateViewModel
    {
        [Inject] public IUIFactory _uiManager; // UI effect 생성용 Interface
        [Inject] public ILoadManager _loadManager; // Scene Load 관리 interface

        public event Action OnDataChanged; // 데이터가 변경될떄 호출될 액션

        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        private void NotifyViewDataChanged() {
            OnDataChanged?.Invoke();
        }

        public PanelType CurrentActivePanel { get; private set; } = PanelType.World; //  어떤 패널이 현재 활성화되어야 하는지를 나타내는 상태 속성
        public PanelType PreActivePanel { get; private set; } = PanelType.World; // 이전 패널이 같은 타입인지 확인용 속성
        public enum PanelType {
            World,
            Upgrade,
        }


        /// <summary>
        /// Panel 버튼 클릭 ex) 월드, 업그레이드 버튼 클릭시 이동
        /// </summary>
        /// <param name="panelType"></param>
        public void OnClickPanelMoveButton(PanelType panelType) {
            CurrentActivePanel = panelType; // 패널 이동
            NotifyViewDataChanged();
            
            // Last pre 패널 갱신
            PreActivePanel = panelType; // 
        }

        /// <summary>
        /// Play Button 클릭
        /// </summary>
        public void OnClickPlayButton() {
            IWipeUI wipeUI = _uiManager.InstanceUI<WipeUI>(20); // UI 생성 요청
            float loadDelay = 0.5f;
            wipeUI.Wipe(WipeDirection.Left, loadDelay, false); // 이펙트 호출
            _loadManager.LoadScene(SceneName.PlayScene, loadDelay); // 다음 씬을 호출
        }
    }
} 
