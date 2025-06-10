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
        /// Ŭ�� ������ / �ʼ��� ���� �Ǵ� ���: Ŭ�� ���� (�ð�, ��ġ), ��Ȳ�� �´� Ŭ�� Ÿ�� 
        /// </summary>
        public abstract void UpdateInput();

        /// <summary>
        /// ȭ���� ray�� ���� tower�� ����
        /// </summary>
        protected bool TryRaycastTowerAtScreenPos(float3 screenPos, out RaycastHit hitInfo) {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            return Physics.Raycast(ray, out hitInfo, 1000f, _towerMask);
        }

    }
}
