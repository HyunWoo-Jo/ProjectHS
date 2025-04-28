using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay
{
    /// <summary>
    /// UI가 아닌 화면 클릭으로 들어오는 인풋을 제어하는 System
    /// </summary>
    public class ScreenClickInputSystem : MonoBehaviour
    {
        private IInputStrategy _inputStrategy;
        public event Action<Vector2> OnInputDragEvent;
        public void SetInputStrategy(IInputStrategy inputStrategy) {  
            _inputStrategy = inputStrategy; 
        }

        private void Update() {

            _inputStrategy.UpdateInput();

            if (_inputStrategy.GetInputType() != InputType.None) {
                Vector2 direction = _inputStrategy.GetPosition() - _inputStrategy.GetFirstFramePosition();

                // Drag
                if(Screen.height / 5 <= direction.magnitude) { // 스크린 높이 기준 20% 보다 크면
                    OnInputDragEvent?.Invoke(direction);
                }


            } 

        }
    }
}
