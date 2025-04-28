using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.InputField;

namespace GamePlay
{
    public enum InputType {
        None, // 터치가 없는상태
        First, // Down First 프레임
        Push, // 누르고 잇는중
        End, // Up End 프레임
    }

    public interface IInputStrategy {

        void UpdateInput(); // 인풋 갱신
        Vector2 GetFirstFramePosition(); // 처음 프레임 위치
        Vector2 GetPosition(); // 마지막 프레임 위치
        InputType GetInputType(); // 인풋의 종류
        float ClickTime(); // 클릭 지속 시간
    }


    public abstract class InputBase : IInputStrategy {
        protected Vector2 firstFramePosition;
        protected float clickStartTime;
        protected InputType inputType;

        public virtual float ClickTime() {
            return Time.time - clickStartTime;
        }

        public virtual Vector2 GetFirstFramePosition() {
            return firstFramePosition;
        }

        public  InputType GetInputType() {
            return inputType;
        }

        public abstract Vector2 GetPosition();

        /// <summary>
        /// 클릭 구현부 / 필수로 들어가야 되는 목록: 클릭 시작 (시간, 위치), 상황에 맞는 클릭 타입 
        /// </summary>
        public abstract void UpdateInput();
    }

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
            } else if(inputType == InputType.End) { 
                inputType = InputType.None;
            }
        }

        public override Vector2 GetPosition() {
            return Input.mousePosition;
        }
    }

    public class MobileInputStrategy : InputBase {
        private Touch? _touch = null;
        public override void UpdateInput() {
            _touch = null;
            foreach (var touch in Input.touches) { // 터치 목록 검색
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // UI 클릭이 아닌 첫번째 터치 목록을 받아옴
                    _touch = touch;
                    break;
                }
            }
            if (_touch.HasValue) {
                Touch touch = _touch.Value;
                if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // 시작할때 등록 UI 클릭이면 등록하지 않음
                    inputType = InputType.First;
                    firstFramePosition = touch.position;
                    clickStartTime = Time.time;
                }
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                    inputType = InputType.Push;
                }
                // 해당 터치가 이번 프레임에 끝났는지 확인 (손가락을 뗐거나 취소됨)
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                    inputType = InputType.End;
                }
            } else {
                inputType = InputType.None;
            }
           
        }
        public override Vector2 GetPosition() {
            return _touch.HasValue ? _touch.Value.position : Vector2.zero;
        }
    }
}
