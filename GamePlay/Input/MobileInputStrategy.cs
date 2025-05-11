using UnityEngine;
using UnityEngine.EventSystems;
using Data;
namespace GamePlay
{
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
