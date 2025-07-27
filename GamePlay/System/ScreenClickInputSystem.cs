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
    /// UI가 아닌 화면 클릭으로 들어오는 인풋을 제어하는 System
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class ScreenClickInputSystem : MonoBehaviour
    {
        private IInputStrategy _inputStrategy;
        public event Action<Vector2> OnInputDragEvent; // 드래그 이벤트
        public event Action<float> OnCloseUpDownEvent; // 확대 축소 이벤트
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
                Debug.LogError("SetInputStrategy 를 반드시 호출해야함"); // 예외
                return;
            }
            _inputStrategy.UpdateInput(); // Update
            
            InputType inputType = _inputStrategy.GetInputType();
            InputTargetType inputTargetType = _inputStrategy.GetInputTargetType();
            if (inputType != InputType.None) { // 클릭           
                if (inputTargetType == InputTargetType.Ground) { // Ground 일경우
                    // Drag 이벤트 조건 확인
                    Vector2 direction = _inputStrategy.GetPosition() - _inputStrategy.GetFirstFramePosition();
                    if (_screenHeight * _groundDragThresholdPct <= direction.magnitude) {
                        OnInputDragEvent?.Invoke(direction);
                    }
                } else if (inputTargetType == InputTargetType.Tower) { // Tower인 경우
                    // 히트 이벤트 발생                                                                    
                    GameObject hitObj = _inputStrategy.GetHitObject();
                    OnRayHitEvent?.Invoke(hitObj);
                    HandleEdgeMove(_inputStrategy.GetPosition());
                }
                

            }
            // 터치를 땟을 경우
            if(inputType == InputType.End) {
                OnUpPointEvent?.Invoke();
            }

            // 확대, 축소 조건 확인
            float closeUpDownSize = _inputStrategy.GetCloseUpDownSizeSize();
            if (Math.Abs(closeUpDownSize) > math.EPSILON) {
                OnCloseUpDownEvent?.Invoke(closeUpDownSize);
            }

        }
        private void HandleEdgeMove(Vector2 screenPos) {
            // 0~1 정규화
            float px = screenPos.x / _screenWidth;  
            float py = screenPos.y / _screenHeight; 

            Vector2 dir = Vector2.zero;

            if (px >= _edgeMovePct) dir.x = 1;   // 오른쪽
            else if (px <= 1f - _edgeMovePct) dir.x = -1;  // 왼쪽

            if (py >= _edgeMovePct) dir.y = 1;   // 위
            else if (py <= 1f - _edgeMovePct) dir.y = -1;  // 아래

            if (dir == Vector2.zero) return;           // 경계 안이라면 종료

            // 이벤트로 전달
            OnInputDragEvent?.Invoke(dir.normalized);
        }
    }
}
