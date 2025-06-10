using UnityEngine;
using UnityEngine.EventSystems;
using Data;

namespace GamePlay
{
   

    public interface IInputStrategy {

        void UpdateInput(); // ��ǲ ����
        Vector2 GetFirstFramePosition(); // ó�� ������ ��ġ
        Vector2 GetPosition(); // ������ ������ ��ġ
        InputType GetInputType(); // ��ǲ�� ����
        InputTargetType GetInputTargetType(); // ��ǲ�� �������� ��� �κ��� Ŭ���߳�
        float ClickTime(); // Ŭ�� ���� �ð�
        float GetCloseUpDownSizeSize(); // Ȯ�� ��� ����
        GameObject GetHitObject(); // Raycast hit ����


    }


    

}
