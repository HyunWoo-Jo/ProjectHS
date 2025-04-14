using UnityEngine;
using Utility;

/*
���� ���� ����	    �ε� �� / ���� �� / �Ͻ����� / Ŭ���� ��
�� ��ȯ ó��	    �� �̵� ��û ó��, Transition UI ���� ��
���� ����/�� ó��	���� �ʱ�ȭ, ���� ���� üũ, Ŭ����/���� ���� üũ
������ �ε�	        SaveData, PlayerData �� �ҷ�����
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
    /// ���� ���� ����
    /// </summary>
    /// <param name="state"></param>
    public void ChangeGameState(GameState state) {
        State = state;
    }

    protected override void Awake() {
        base.Awake();

    }




}
