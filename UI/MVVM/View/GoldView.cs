
using UnityEngine;
using Zenject;
using System;
using TMPro;
using ModestTree;
using Data;
using System.Diagnostics;
using R3;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class GoldView : MonoBehaviour
    {
        [Inject] private GoldViewModel _viewModel;
        [SerializeField] private TextMeshProUGUI _goldText;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // UI Bind
            _viewModel.RO_GoldObservable
                .ThrottleLastFrame(1)
                .Subscribe(UpdateUI)
                .AddTo(this);
            
        }
        private void Start() {
            _viewModel.Notify();
        }


#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_goldText);
        }
#endif
        // UI 갱신
        private void UpdateUI(int value) {
            _goldText.text = GoldStyle.GoldToString(value);
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
