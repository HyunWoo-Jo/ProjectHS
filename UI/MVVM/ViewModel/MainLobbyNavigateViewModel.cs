
using Data;
using System.Diagnostics;
using System;
using Zenject;

namespace UI
{
    public class MainLobbyNavigateViewModel
    {
        [Inject] public IUIFactory _uiManager;
        [Inject] public IMainCanvasTag _mainCanvasTag;
        [Inject] public ILoadManager _loadManager;

        public event Action OnDataChanged; // �����Ͱ� ����ɋ� ȣ��� �׼�

        /// <summary>
        /// ������ ���� �˸�
        /// </summary>
        private void NotifyViewDataChanged() {
            OnDataChanged?.Invoke();
        }

        public PanelType CurrentActivePanel { get; private set; } = PanelType.World; //  � �г��� ���� Ȱ��ȭ�Ǿ�� �ϴ����� ��Ÿ���� ���� �Ӽ�
        public PanelType PreActivePanel { get; private set; } = PanelType.World; // ���� �г��� ���� Ÿ������ Ȯ�ο� �Ӽ�
        public enum PanelType {
            World,
            Upgrade,
        }
        /// <summary>
        /// Panel ��ư Ŭ��
        /// </summary>
        /// <param name="panelType"></param>
        public void OnClickPanelMoveButton(PanelType panelType) {
            CurrentActivePanel = panelType; // �г� �̵�
            NotifyViewDataChanged();
            
            // Last
            PreActivePanel = panelType; // 
        }

        /// <summary>
        /// Play Button Ŭ��
        /// </summary>
        public void OnClickPlayButton() {
            IWipeUI wipeUI = _uiManager.InstanceUI<WipeUI>(_mainCanvasTag.GetMainCanvas().transform, 20); // UI ���� ��û
            float loadDelay = 0.5f;
            wipeUI.Wipe(WipeDirection.Left, loadDelay, false); // ����Ʈ ȣ��
            _loadManager.LoadScene(SceneName.PlayScene, loadDelay); // ���� ���� ȣ��
        }
    }
} 
