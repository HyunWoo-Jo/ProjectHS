using UnityEngine;
using UnityEngine.EventSystems;
using Data;
namespace GamePlay
{
    public class PcInputStrategy : InputBase {

        public override void UpdateInput() {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) { // UI 위 클릭이 아닐경우
                inputType = InputType.First;
                clickStartTime = Time.time;
                firstFramePosition = GetPosition();
            } else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) { // Push중
                inputType = InputType.Push;
            }
            if (Input.GetMouseButtonUp(0) && (inputType == InputType.First || inputType == InputType.Push)) { // UP 
                inputType = InputType.End;
            } else if (inputType == InputType.End) {
                inputType = InputType.None;
            }
        }

        public override Vector2 GetPosition() {
            return Input.mousePosition;
        }
    }

}
