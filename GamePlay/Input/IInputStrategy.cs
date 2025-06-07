using UnityEngine;
using UnityEngine.EventSystems;
using Data;

namespace GamePlay
{
   

    public interface IInputStrategy {

        void UpdateInput(); // 인풋 갱신
        Vector2 GetFirstFramePosition(); // 처음 프레임 위치
        Vector2 GetPosition(); // 마지막 프레임 위치
        InputType GetInputType(); // 인풋의 종류
        float ClickTime(); // 클릭 지속 시간
        float GetCloseUpDownSizeSize(); // 확대 축소 정보
        
    }


    

}
