
using Data;
using System.Diagnostics;
using System;
using Zenject;
using Contracts;
using R3;

namespace UI
{
    // 메인로비의 Navigate UI 를 관리
    public class MainLobbyNavigateViewModel
    {
        [Inject] private ISceneTransitionService _sts;

        private ReactiveProperty<PanelType> _currentActivePanelObservable = new();//  어떤 패널이 현재 활성화되어야 하는지를 나타내는 상태 속성
        public ReadOnlyReactiveProperty<PanelType> RO_CurrentActivePanelObservable => _currentActivePanelObservable;
        public PanelType CurrentActivePanel => _currentActivePanelObservable.CurrentValue;
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
            _currentActivePanelObservable.Value = panelType;
            // Last pre 패널 갱신
            PreActivePanel = panelType; // 
        }

        public void ChangeScene() {
            _sts.LoadScene(SceneName.PlayScene);
        }

       
    }
} 
