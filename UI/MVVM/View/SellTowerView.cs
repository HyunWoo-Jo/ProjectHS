
using UnityEngine;
using Zenject;
using System;
using TMPro;
using UnityEngine.Assertions;
using Data;
using UnityEngine.EventSystems;
using CustomUtility;
using Contracts;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class SellTowerView : MonoBehaviour
    {
        [Inject] private SellTowerViewModel _viewModel;
        [SerializeField] private TextMeshProUGUI _sellCostText;
        [SerializeField] private PointerUpEventRelay _eventArea;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnDataChanged += UpdateCostUI;

            ButtonInit();
            // Event 영역에 Handler Bind
            _eventArea.OnPointerUpEvent += OnPointerUP;
        }

        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdateCostUI;
            _eventArea.OnPointerUpEvent -= OnPointerUP;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_sellCostText);
            Assert.IsNotNull(_eventArea);
        }
#endif
        // 버튼 초기화
        private void ButtonInit() {
            string className = GetType().Name;
            this.gameObject.AddEventTracker(className, nameof(OnPointerUP));
            // Input Strategy를 통해 OnPointerUP에서 호출이 일어남
        }

        // UI 갱신
        private void UpdateCostUI(int cost) {
            _sellCostText.text = GoldStyle.GoldToString(cost);
        }

        /// <summary>
        /// 버튼
        /// </summary>
        public void OnPointerUP() {
            _viewModel.TrySell();
        }
        ////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
