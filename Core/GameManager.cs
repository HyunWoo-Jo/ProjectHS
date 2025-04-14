using UnityEngine;
using Utility;

/*
게임 상태 관리	    로딩 중 / 게임 중 / 일시정지 / 클리어 등
씬 전환 처리	    씬 이동 요청 처리, Transition UI 연결 등
게임 시작/끝 처리	게임 초기화, 시작 조건 체크, 클리어/실패 조건 체크
데이터 로딩	        SaveData, PlayerData 등 불러오기
*/
public class GameManager : Singleton<GameManager>
{
    public enum GameState {
        Running,
        Pause,
        Clear,
        Loading,
    }

    public GameState State {  get; private set; }

    /// <summary>
    /// 게임 상태 변경
    /// </summary>
    /// <param name="state"></param>
    public void ChangeGameState(GameState state) {
        State = state;
    }

    protected override void Awake() {
        base.Awake();

    }




}
