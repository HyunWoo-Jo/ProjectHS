
using UnityEngine;
using TMPro;
using Zenject;
using ModestTree;
using R3;
using System;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class CrystalView : MonoBehaviour
    {
        [Inject] private CrystalViewModel _viewModel;

        [SerializeField] private TextMeshProUGUI _text;

        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // UI Bind

            _viewModel.RO_CrystalObservable
                .ThrottleLastFrame(1)
                .ObserveOnMainThread()
                .Subscribe(UpdateUI)
                .AddTo(this);
        }

        private void Start() {
            _viewModel.Notify();
        }


#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_text);
        }
#endif
        // UI 갱신
        private void UpdateUI(int value) {
            _text.text = value.ToString();
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
