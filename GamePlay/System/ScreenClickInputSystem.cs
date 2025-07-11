using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Data;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;

namespace GamePlay
{
    /// <summary>
    /// UI�� �ƴ� ȭ�� Ŭ������ ������ ��ǲ�� �����ϴ� System
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class ScreenClickInputSystem : MonoBehaviour
    {
        private IInputStrategy _inputStrategy;
        public event Action<Vector2> OnInputDragEvent; // �巡�� �̺�Ʈ
        public event Action<float> OnCloseUpDownEvent; // Ȯ�� ��� �̺�Ʈ
        public event Action<GameObject> OnRayHitEvent; 

        public event Action OnUpPointEvent; 

        private float _screenHeight;
        private float _screenWidth;

        [SerializeField][Range(0, 1)] private float _groundDragThresholdPct = 0.2f;
        [SerializeField][Range(0, 1)] private float _edgeMovePct = 0.9f;

        public void SetInputStrategy(IInputStrategy inputStrategy) {  
            _inputStrategy = inputStrategy; 
        }

        public void OnEnable() {
            _screenHeight = (float)Screen.height;
            _screenWidth = (float)Screen.width;
        }


        private void Update() {
            if (GameSettings.IsPause) return;
            if (_inputStrategy == null) {
                Debug.LogError("SetInputStrategy �� �ݵ�� ȣ���ؾ���"); // ����
                return;
            }
            _inputStrategy.UpdateInput(); // Update
            
            InputType inputType = _inputStrategy.GetInputType();
            InputTargetType inputTargetType = _inputStrategy.GetInputTargetType();
            if (inputType != InputType.None) { // Ŭ��           
                if (inputTargetType == InputTargetType.Ground) { // Ground �ϰ��
                    // Drag �̺�Ʈ ���� Ȯ��
                    Vector2 direction = _inputStrategy.GetPosition() - _inputStrategy.GetFirstFramePosition();
                    if (_screenHeight * _groundDragThresholdPct <= direction.magnitude) {
                        OnInputDragEvent?.Invoke(direction);
                    }
                } else if (inputTargetType == InputTargetType.Tower) { // Tower�� ���
                    // ��Ʈ �̺�Ʈ �߻�                                                                    
                    GameObject hitObj = _inputStrategy.GetHitObject();
                    OnRayHitEvent?.Invoke(hitObj);
                    HandleEdgeMove(_inputStrategy.GetPosition());
                }
                

            }
            // ��ġ�� ���� ���
            if(inputType == InputType.End) {
                OnUpPointEvent?.Invoke();
            }

            // Ȯ��, ��� ���� Ȯ��
            float closeUpDownSize = _inputStrategy.GetCloseUpDownSizeSize();
            if (Math.Abs(closeUpDownSize) > math.EPSILON) {
                OnCloseUpDownEvent?.Invoke(closeUpDownSize);
            }

        }
        private void HandleEdgeMove(Vector2 screenPos) {
            // 0~1 ����ȭ
            float px = screenPos.x / _screenWidth;  
            float py = screenPos.y / _screenHeight; 

            Vector2 dir = Vector2.zero;

            if (px >= _edgeMovePct) dir.x = 1;   // ������
            else if (px <= 1f - _edgeMovePct) dir.x = -1;  // ����

            if (py >= _edgeMovePct) dir.y = 1;   // ��
            else if (py <= 1f - _edgeMovePct) dir.y = -1;  // �Ʒ�

            if (dir == Vector2.zero) return;           // ��� ���̶�� ����

            // �̺�Ʈ�� ����
            OnInputDragEvent?.Invoke(dir.normalized);
        }
    }
}
