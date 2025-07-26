
using UnityEngine;
using Zenject;
using System;
using UnityEngine.EventSystems;
using ModestTree;
using TMPro;
using CustomUtility;
using System.Runtime.CompilerServices;
using Data;
using R3;
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
            _viewModel.RO_TowerPriceObservable
                .ThrottleLastFrame(1)
                .Subscribe(UpdateUI)
                .AddTo(this);

            // 버튼 초기화
            _button.ToObservableEventTrigger(GetType().Name, nameof(OnClickPurchase))
                .OnPointerClickAsObservable()
                .ThrottleFirstFrame(1)
                .Subscribe(_ => OnClickPurchase())
                .AddTo(this);
        }
        private void Start() {
            _viewModel.Notify();
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
            _priceText.text = GoldStyle.GoldToString(price);
        }

        private void OnClickPurchase() {
            if (_viewModel.TryPurchase()) { // 구매 성공

            } else { // 실패 

            }
        }

    }
}
