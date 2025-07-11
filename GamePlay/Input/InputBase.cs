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
        /// Ŭ�� ������ / �ʼ��� ���� �Ǵ� ���: Ŭ�� ���� (�ð�, ��ġ), ��Ȳ�� �´� Ŭ�� Ÿ�� 
        /// </summary>
        public abstract void UpdateInput();

        /// <summary>
        /// ȭ���� ray�� ���� Layer�� ����
        /// </summary>
        protected bool TryRaycastAtScreenPos(LayerMask layer, float3 screenPos, out RaycastHit hitInfo) {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            return Physics.Raycast(ray, out hitInfo, 1000f, layer);
        }
        /// <summary>
        /// screen ��ġ�� UI�� ����
        /// </summary>
        protected bool TryUIRaycast(Vector2 screenPos, out RaycastResult hit) {
            var eventData = new PointerEventData(EventSystem.current) {
                position = screenPos          // ���콺����ġ ��ǥ
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0) {
                hit = results[0];              // �ֻ�� UI
                return true;
            }

            hit = default;
            return false;
        }
    }
}
