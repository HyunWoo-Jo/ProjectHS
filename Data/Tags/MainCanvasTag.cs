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

        private void Awake() {
            _canvas = GetComponent<Canvas>();
            _rectTr = GetComponent<RectTransform>();

            _uiManager.SetMainCanvas(this); // Main Cavnas·Î µî·Ï
        }
        public Canvas GetMainCanvas() {
            return _canvas;
        }

        public RectTransform GetRectTransform() {
            return _rectTr;
        }
    }
}
