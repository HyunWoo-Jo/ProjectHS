using UnityEngine;
using Data;
namespace GamePlay
{
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

        public InputType GetInputType() {
            return inputType;
        }

        public abstract Vector2 GetPosition();

        /// <summary>
        /// Ŭ�� ������ / �ʼ��� ���� �Ǵ� ���: Ŭ�� ���� (�ð�, ��ġ), ��Ȳ�� �´� Ŭ�� Ÿ�� 
        /// </summary>
        public abstract void UpdateInput();
    }
}
