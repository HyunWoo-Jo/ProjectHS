using UnityEngine;
using UnityEngine.Assertions;

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


        private void Awake() {
            _canvas = GetComponent<Canvas>();
            _rectTr = GetComponent<RectTransform>();
        }
        public Canvas GetMainCanvas() {
            return _canvas;
        }

        public RectTransform GetRectTransform() {
            return _rectTr;
        }
    }
}
