
using UnityEngine;
using Zenject;
using System;
using UnityEngine.EventSystems;
using ModestTree;
using TMPro;
using CustomUtility;
using System.Runtime.CompilerServices;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class TowerPurchaseView : MonoBehaviour
    {
        [Inject] private TowerPurchaseViewModel _viewModel;
        [SerializeField] private EventTrigger _button;
        [SerializeField] private TextMeshProUGUI _priceText;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnDataChanged += UpdateUI;

            ButtonInit();
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
            Assert.IsNotNull(_button);
            Assert.IsNotNull(_priceText);
        }
#endif
        // UI 갱신
        private void UpdateUI(int price) {
            _priceText.text = price.ToString() + "G";
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

        private void ButtonInit() {
            _button.AddTrigger<bool>(EventTriggerType.PointerClick, _viewModel.PurchaseButtonClick, OnPurchaseButton, GetType().Name, nameof(ButtonInit));
        }

        private void OnPurchaseButton(bool isSuccess) {
            if (isSuccess) { // 구매 성공 UI 이벤트

            } else { // 구매 실패 UI 이벤트

            }
        }

    }
}
