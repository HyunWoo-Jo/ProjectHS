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
        /// 클릭 구현부 / 필수로 들어가야 되는 목록: 클릭 시작 (시간, 위치), 상황에 맞는 클릭 타입 
        /// </summary>
        public abstract void UpdateInput();
    }
}
