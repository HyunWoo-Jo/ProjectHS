using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay
{
    /// <summary>
    /// UI�� �ƴ� ȭ�� Ŭ������ ������ ��ǲ�� �����ϴ� System
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
                if(Screen.height / 5 <= direction.magnitude) { // ��ũ�� ���� ���� 20% ���� ũ��
                    OnInputDragEvent?.Invoke(direction);
                }


            } 

        }
    }
}
