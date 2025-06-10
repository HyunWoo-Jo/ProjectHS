using UnityEngine;
using Data;
using Unity.Mathematics;
namespace GamePlay
{
    public abstract class InputBase : IInputStrategy {
        protected Vector2 firstFramePosition;
        protected float clickStartTime;
        protected InputType inputType;
        protected InputTargetType inputTargetType;
        protected float closeUpDownSize;
        protected GameObject hitObject;
        private readonly LayerMask _towerMask = LayerMask.GetMask("Tower");

        protected InputBase() {
            Debug.Log(_towerMask.value);
        }

        public virtual float ClickTime() {
            return Time.time - clickStartTime;
        }

        public virtual Vector2 GetFirstFramePosition() => firstFramePosition;

        public InputType GetInputType() => inputType;
        public InputTargetType GetInputTargetType() => inputTargetType;

        public float GetCloseUpDownSizeSize() => closeUpDownSize;
         
        public GameObject GetHitObject() => hitObject;


        public abstract Vector2 GetPosition();

        /// <summary>
        /// 클릭 구현부 / 필수로 들어가야 되는 목록: 클릭 시작 (시간, 위치), 상황에 맞는 클릭 타입 
        /// </summary>
        public abstract void UpdateInput();

        /// <summary>
        /// 화면의 ray를 쏴서 tower를 검출
        /// </summary>
        protected bool TryRaycastTowerAtScreenPos(float3 screenPos, out RaycastHit hitInfo) {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            return Physics.Raycast(ray, out hitInfo, 1000f, _towerMask);
        }

    }
}
