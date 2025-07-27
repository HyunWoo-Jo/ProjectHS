
using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using CustomUtility;
using R3;
using Contracts;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class MainLobbyUpgradeView : MonoBehaviour
    {
        [Inject] private MainLobbyUpgradeViewModel _viewModel;

        private Dictionary<GlobalUpgradeType, LobbyUpgradeSlot> _slotsDict = new();

        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // slot 검색
            foreach (LobbyUpgradeSlot slot in GetComponentsInChildren<LobbyUpgradeSlot>()) {
                _slotsDict.Add(slot.GetUpgradeType(), slot);
            }
            Bind();
            ButtonInit();
        }


#if UNITY_EDITOR
        // 검증
        private void RefAssert() {

        }
#endif

        private void Bind() {
            foreach (var slot in _slotsDict) {
                var type = slot.Value.GetUpgradeType();
                _viewModel.GetRO_UpgradeData(type)
                    .ThrottleLastFrame(1)
                    .SubscribeOnMainThread()
                    .Subscribe(level => {
                        UpdateUI(type);
                    })
                    .AddTo(this);

            }
        }


        // 초기화
        private void ButtonInit() {
            string className = GetType().Name;
            string methodName = nameof(ButtonInit);
            foreach (var slot in _slotsDict) {
                // 버튼에 기능 추가
                var entTrigger = slot.Value.GetEventTrigger();
                entTrigger.ToObservableEventTrigger(className, methodName)
                    .OnPointerClickAsObservable()
                    .ThrottleFirstFrame(1)
                    .Subscribe(_ => _viewModel.TryPurchase(slot.Value.GetUpgradeType()))
                    .AddTo(this);
            }
        }

        // UI 갱신
        private void UpdateUI(GlobalUpgradeType type) {
            var slot = _slotsDict[type];

            int price = _viewModel.GetPrice(type);
            int abliltyValue = _viewModel.GetAbilityValue(type);

            // UI 수정
            slot.SetValueText((abliltyValue).ToString()); // Text 변경
            slot.SetPriceText((price).ToString());
        }



    }
}
