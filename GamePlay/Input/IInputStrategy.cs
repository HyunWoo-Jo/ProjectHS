using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.InputField;

namespace GamePlay
{
    public enum InputType {
        None, // ��ġ�� ���»���
        First, // Down First ������
        Push, // ������ �մ���
        End, // Up End ������
    }

    public interface IInputStrategy {

        void UpdateInput(); // ��ǲ ����
        Vector2 GetFirstFramePosition(); // ó�� ������ ��ġ
        Vector2 GetPosition(); // ������ ������ ��ġ
        InputType GetInputType(); // ��ǲ�� ����
        float ClickTime(); // Ŭ�� ���� �ð�
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
        /// Ŭ�� ������ / �ʼ��� ���� �Ǵ� ���: Ŭ�� ���� (�ð�, ��ġ), ��Ȳ�� �´� Ŭ�� Ÿ�� 
        /// </summary>
        public abstract void UpdateInput();
    }

    public class PcInputStrategy : InputBase {
      
        public override void UpdateInput() {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) { // UI �� Ŭ���� �ƴҰ��
                inputType = InputType.First;
                clickStartTime = Time.time;
                firstFramePosition = GetPosition();
            } else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) { // Push��
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
