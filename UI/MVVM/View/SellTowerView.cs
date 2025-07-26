
using UnityEngine;
using Zenject;
using System;
using TMPro;
using UnityEngine.Assertions;
using Data;
using UnityEngine.EventSystems;
using CustomUtility;
using Contracts;
using R3;
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
            // UI Bind
            _viewModel.RO_CostObservable
                .Subscribe(UpdateCostUI)
                .AddTo(this);

            // 버튼 초기화
            string className = GetType().Name;
            this.gameObject.AddEventTracker(className, nameof(OnPointerUP));
            // Event 영역에 Handler Bind
            _eventArea.OnPointerUpEvent += OnPointerUP;
        }


#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_sellCostText);
            Assert.IsNotNull(_eventArea);
        }
#endif


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
