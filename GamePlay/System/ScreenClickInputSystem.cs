using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Data;

namespace GamePlay
{
    /// <summary>
    /// UI가 아닌 화면 클릭으로 들어오는 인풋을 제어하는 System
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class ScreenClickInputSystem : MonoBehaviour
    {
        private IInputStrategy _inputStrategy;
        public event Action<Vector2> OnInputDragEvent;
        public void SetInputStrategy(IInputStrategy inputStrategy) {  
            _inputStrategy = inputStrategy; 
        }

        private void Update() {
            if (_inputStrategy == null) {
                Debug.LogError("SetInputStrategy 를 반드시 호출해야함"); // 예외
                return;
            }
            _inputStrategy.UpdateInput();

            if (_inputStrategy.GetInputType() != InputType.None) {
                Vector2 direction = _inputStrategy.GetPosition() - _inputStrategy.GetFirstFramePosition();

                // Drag
                if((float)Screen.height * 0.20f <= direction.magnitude) { // 스크린 높이 기준 20% 보다 크면
                    OnInputDragEvent?.Invoke(direction);
                }


            } 

        }
    }
}
