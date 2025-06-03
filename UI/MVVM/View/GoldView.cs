
using UnityEngine;
using Zenject;
using System;
using TMPro;
using ModestTree;
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
            // 버튼 초기화
            _viewModel.OnDataChanged += UpdateUI;

        }
        private void Start() {
            _viewModel.Update();
        }
        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdateUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_goldText);
        }
#endif
        // UI 갱신
        private void UpdateUI(int value) {
            _goldText.text = value.ToString();
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
