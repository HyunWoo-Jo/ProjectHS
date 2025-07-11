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
    게임 상태 관리	    로딩 중 / 게임 중 / 일시정지 / 클리어 등
    씬 전환 처리	    씬 이동 요청 처리, Transition UI 연결 등
    게임 시작/끝 처리	게임 초기화, 시작 조건 체크, 클리어/실패 조건 체크
    데이터 로딩	        SaveData, PlayerData 등 불러오기
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
        /// 게임 상태 변경
        /// </summary>
        /// <param name="state"></param>
        public void ChangeGameState(GameState state) {
            State = state;
        }

        private void Awake() {
            // Scene 전환시 이벤트가 발생되도록 등록
            SceneManager.sceneLoaded += LoadSceneEffect;

            Application.targetFrameRate = 120; // 타겟 프레임 설정

        }

        /// <summary>
        /// 씬 전환이 일어날때 이펙트 발생
        /// </summary>
        private void LoadSceneEffect(Scene scene, LoadSceneMode mode) {
            if (SceneManager.GetActiveScene().name != SceneName.LoadScene.ToString() && 
                _loadManager.GetPreSceneName() != _loadManager.GetNextSceneName()) { // 로드 씬이 아닐경우 , 처음 로드한 씬이 아닌경우 이펙트 실행
                IWipeUI wipeUI = _uiFactory.InstanceUI<WipeUI>(100);
                wipeUI.Wipe(WipeDirection.FillRight, 0.5f, true);

            }
        }

  
       




    }

}
