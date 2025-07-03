using Data;
using GamePlay;
using System;
using System.Linq;
using System.Collections.Generic;   
using UnityEngine;
using CustomUtility;
using Zenject;
using UI;
using UnityEngine.SceneManagement;

namespace GamePlay {
    /*
    ���� ���� ����	    �ε� �� / ���� �� / �Ͻ����� / Ŭ���� ��
    �� ��ȯ ó��	    �� �̵� ��û ó��, Transition UI ���� ��
    ���� ����/�� ó��	���� �ʱ�ȭ, ���� ���� üũ, Ŭ����/���� ���� üũ
    ������ �ε�	        SaveData, PlayerData �� �ҷ�����
    */
    public class GameManager : MonoBehaviour {
        public enum GameState {
            Running,
            Pause,
            Clear,
            Loading,
        }

        public GameState State { get; private set; }
        public MapTema MapTema { get; private set; }


        [Inject] IUIFactory _uiFactory;
        [Inject] ILoadManager _loadManager;

        public Vector2Int MapSize { get; private set; } = new Vector2Int(20, 20);

        /// <summary>
        /// ���� ���� ����
        /// </summary>
        /// <param name="state"></param>
        public void ChangeGameState(GameState state) {
            State = state;
        }

        private void Awake() {
            // Scene ��ȯ�� �̺�Ʈ�� �߻��ǵ��� ���
            SceneManager.sceneLoaded += LoadSceneEffect;

            Application.targetFrameRate = 120; // Ÿ�� ������ ����

        }

        /// <summary>
        /// �� ��ȯ�� �Ͼ�� ����Ʈ �߻�
        /// </summary>
        private void LoadSceneEffect(Scene scene, LoadSceneMode mode) {
            if (SceneManager.GetActiveScene().name != SceneName.LoadScene.ToString() && 
                _loadManager.GetPreSceneName() != _loadManager.GetNextSceneName()) { // �ε� ���� �ƴҰ�� , ó�� �ε��� ���� �ƴѰ�� ����Ʈ ����
                IWipeUI wipeUI = _uiFactory.InstanceUI<WipeUI>(100);
                wipeUI.Wipe(WipeDirection.FillRight, 0.5f, true);

            }
        }

  
       




    }

}
