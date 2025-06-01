
using UnityEngine;
using Zenject;
using System;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class PurchaseTowerView : MonoBehaviour
    {
        [Inject] private PurchaseTowerViewModel _viewModel;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnDataChanged += UpdateUI;

        }

        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdateUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {

        }
#endif
        // UI 갱신
        private void UpdateUI() {
            
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
