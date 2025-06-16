
using Data;
using System.Diagnostics;
using System;
using Zenject;

namespace UI
{
    // ���ηκ��� Navigate UI �� ����
    public class MainLobbyNavigateViewModel
    {
        public event Action OnDataChanged; // �����Ͱ� ����ɋ� ȣ��� �׼�
        [Inject] public ILoadManager _loadManager; // Scene Load ���� interface

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
        /// Panel ��ư Ŭ�� ex) ����, ���׷��̵� ��ư Ŭ���� �̵�
        /// </summary>
        /// <param name="panelType"></param>
        public void OnClickPanelMoveButton(PanelType panelType) {
            CurrentActivePanel = panelType; // �г� �̵�
            NotifyViewDataChanged();
            
            // Last pre �г� ����
            PreActivePanel = panelType; // 
        }

        public void ChangedScene(float delay) {
            _loadManager.LoadScene(SceneName.PlayScene, delay); // ���� ���� ȣ��
        }

       
    }
} 
