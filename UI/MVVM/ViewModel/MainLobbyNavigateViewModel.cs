
using Data;
using System.Diagnostics;
using System;
using Zenject;
using Contracts;
using R3;

namespace UI
{
    // ���ηκ��� Navigate UI �� ����
    public class MainLobbyNavigateViewModel
    {
        [Inject] private ISceneTransitionService _sts;

        private ReactiveProperty<PanelType> _currentActivePanelObservable = new();//  � �г��� ���� Ȱ��ȭ�Ǿ�� �ϴ����� ��Ÿ���� ���� �Ӽ�
        public ReadOnlyReactiveProperty<PanelType> RO_CurrentActivePanelObservable => _currentActivePanelObservable;
        public PanelType CurrentActivePanel => _currentActivePanelObservable.CurrentValue;
        public PanelType PreActivePanel { get; private set; } = PanelType.World; // ���� �г��� ���� Ÿ������ Ȯ�ο� �Ӽ�
        public enum PanelType {
            World,
            Upgrade,
        }


        /// <summary>
        /// Panel ��ư Ŭ�� ex) ����, ���׷��̵� ��ư Ŭ���� �̵�
        /// </summary>
        /// <param name="panelType"></param>
        public void OnClickPanelMoveButton(PanelType panelType) {
            _currentActivePanelObservable.Value = panelType;
            // Last pre �г� ����
            PreActivePanel = panelType; // 
        }

        public void ChangeScene() {
            _sts.LoadScene(SceneName.PlayScene);
        }

       
    }
} 
