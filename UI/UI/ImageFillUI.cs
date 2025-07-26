using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI
{
    public class ImageFillUI : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_fillImage);
#endif
        }

        private void OnDisable() { // 초기화
            _fillImage.fillAmount = 1;
        }

        public void UpdateHp(float ratio) {
            _fillImage.fillAmount = ratio;
        }

       


    }
}
