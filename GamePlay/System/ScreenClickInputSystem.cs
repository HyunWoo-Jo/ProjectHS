using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Data;

namespace GamePlay
{
    /// <summary>
    /// UI�� �ƴ� ȭ�� Ŭ������ ������ ��ǲ�� �����ϴ� System
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
                Debug.LogError("SetInputStrategy �� �ݵ�� ȣ���ؾ���"); // ����
                return;
            }
            _inputStrategy.UpdateInput();

            if (_inputStrategy.GetInputType() != InputType.None) {
                Vector2 direction = _inputStrategy.GetPosition() - _inputStrategy.GetFirstFramePosition();

                // Drag
                if((float)Screen.height * 0.20f <= direction.magnitude) { // ��ũ�� ���� ���� 20% ���� ũ��
                    OnInputDragEvent?.Invoke(direction);
                }


            } 

        }
    }
}
