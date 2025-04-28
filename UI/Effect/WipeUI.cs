using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public enum WipeDirection {
        Right,
        Left,
        FillLeft, // ä���������� 0 �������� <-
        FillRight, // ->
    }
    public interface IWipeUI {
        void Wipe(WipeDirection direction, float targetTime, bool isAutoActiveClose);
    }
    public class WipeUI : MonoBehaviour, IWipeUI{

        [Inject] private UIEvent _uiEvent;

        [SerializeField] private Image _wipePanel;
        private WipeDirection _direction = WipeDirection.Left;
        private float _targetTime = 0.5f;
        private float _time = 0;
        private int _progressID = Shader.PropertyToID("_Progress");
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_wipePanel);
#endif
        }
        /// <summary>
        /// ���� 
        /// </summary>
        /// <param name="direction"> ����</param>
        /// <param name="targetTime"> �ð� </param>
        /// <param name="isAutoDestroy"> �ڵ� ������Ʈ ���� </param>
        public void Wipe(WipeDirection direction, float targetTime, bool isAutoDestroy) {
            _direction = direction;
            _targetTime = targetTime;
            StartCoroutine(CoroutineWipe(isAutoDestroy));
        }

        private IEnumerator CoroutineWipe(bool isAutoDestroy) {
            float start = 0f;
            float end = 0f;
            if (_direction == WipeDirection.Right) {
                end = -1;
            } else if (_direction == WipeDirection.Left) {
                end = 1;
            } else if (_direction == WipeDirection.FillLeft) {
                start = -1;
                end = 0;
            } else if (_direction == WipeDirection.FillRight) {
                start = 1;
                end = 0;
            }
            SetMatPro(start);
            while (true) {
                _time += Time.deltaTime;
                // wipe �� 
                float wipe = Mathf.Lerp(start, end, Mathf.Clamp(_time / _targetTime, 0, 1));
                SetMatPro(wipe);
                if (_time >= _targetTime) { break; }
                yield return null;
            }
            if (isAutoDestroy) {
                Close();
            }
        }
        private void SetMatPro(float value) {
            _wipePanel.material.SetFloat(_progressID, value);
        }
       

        public void Close() {
            _uiEvent.CloseUI(this.gameObject); // ���� �̺�Ʈ �߻�
        }

       
    }
}
