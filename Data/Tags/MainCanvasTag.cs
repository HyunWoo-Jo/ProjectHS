using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Data
{
    public interface IMainCanvasTag {
        Canvas GetMainCanvas();
        RectTransform GetRectTransform();
    }
    public class MainCanvasTag : MonoBehaviour ,IMainCanvasTag
    {
        private Canvas _canvas;
        private RectTransform _rectTr;
        [Inject] private ISetMainCanvas _uiManager;
        [Inject] private DiContainer _container;
        private void Awake() {
            _canvas = GetComponent<Canvas>();
            _rectTr = GetComponent<RectTransform>();

            _uiManager.SetMainCanvas(this, _container); // Main Cavnas로 등록
        }
        public Canvas GetMainCanvas() {
            return _canvas;
        }

        public RectTransform GetRectTransform() {
            return _rectTr;
        }
    }
}
