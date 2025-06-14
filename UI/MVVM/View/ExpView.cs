
using UnityEngine;
using Zenject;
using System;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class ExpView : MonoBehaviour
    {
        [Inject] private ExpViewModel _viewModel;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _levelText;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnLevelChanged += UpdateLevelUI;
            _viewModel.OnExpChanged += UpdateExpUI;

            _viewModel.Update();
        }

        private void OnDestroy() {
            _viewModel.OnLevelChanged -= UpdateLevelUI;
            _viewModel.OnExpChanged -= UpdateExpUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull( _fillImage );
        }
#endif

        ////////////////////////////////////////////////////////////////////////////////////
        // your logic here
        private void UpdateExpUI(float value) {
            _fillImage.fillAmount = _viewModel.GetExpRation();
        }
        private void UpdateLevelUI(int value) {
            _levelText.text = "Level " + value.ToString();
        }

    }
}
