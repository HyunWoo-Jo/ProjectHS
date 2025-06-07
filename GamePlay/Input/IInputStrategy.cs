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
        float ClickTime(); // Ŭ�� ���� �ð�
        float GetCloseUpDownSizeSize(); // Ȯ�� ��� ����
        
    }


    

}
