
using UnityEngine;
using Zenject;
using System;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using R3;
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
            // UI Bind
            _viewModel.RO_LevelObservable
                .ThrottleLastFrame(1)
                .Subscribe(UpdateLevelUI)
                .AddTo(this);

            _viewModel.RO_CurExpObservable
                .ThrottleLastFrame(1)
                .Subscribe(UpdateExpUI)
                .AddTo(this);

            _viewModel.Notify();
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
