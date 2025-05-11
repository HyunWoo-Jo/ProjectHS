using UnityEngine;
using UnityEngine.EventSystems;
using Data;
namespace GamePlay
{
    public class MobileInputStrategy : InputBase {
        private Touch? _touch = null;
        public override void UpdateInput() {
            _touch = null;
            foreach (var touch in Input.touches) { // ��ġ ��� �˻�
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // UI Ŭ���� �ƴ� ù��° ��ġ ����� �޾ƿ�
                    _touch = touch;
                    break;
                }
            }
            if (_touch.HasValue) {
                Touch touch = _touch.Value;
                if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // �����Ҷ� ��� UI Ŭ���̸� ������� ����
                    inputType = InputType.First;
                    firstFramePosition = touch.position;
                    clickStartTime = Time.time;
                }
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                    inputType = InputType.Push;
                }
                // �ش� ��ġ�� �̹� �����ӿ� �������� Ȯ�� (�հ����� �ðų� ��ҵ�)
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
