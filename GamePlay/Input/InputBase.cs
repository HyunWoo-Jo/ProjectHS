using UnityEngine;
using Data;
using Unity.Mathematics;
using UnityEngine.EventSystems;
using System.Collections.Generic;
namespace GamePlay
{
    public abstract class InputBase : IInputStrategy {
        protected Vector2 firstFramePosition;
        protected float clickStartTime;
        protected InputType inputType;
        protected InputTargetType inputTargetType;
        protected float closeUpDownSize;
        protected GameObject hitObject;
        protected readonly LayerMask towerMask = LayerMask.GetMask("Tower");

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
        /// 화면의 ray를 쏴서 Layer를 검출
        /// </summary>
        protected bool TryRaycastAtScreenPos(LayerMask layer, float3 screenPos, out RaycastHit hitInfo) {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            return Physics.Raycast(ray, out hitInfo, 1000f, layer);
        }
        /// <summary>
        /// screen 위치에 UI를 검출
        /// </summary>
        protected bool TryUIRaycast(Vector2 screenPos, out RaycastResult hit) {
            var eventData = new PointerEventData(EventSystem.current) {
                position = screenPos          // 마우스·터치 좌표
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0) {
                hit = results[0];              // 최상단 UI
                return true;
            }

            hit = default;
            return false;
        }
    }
}
