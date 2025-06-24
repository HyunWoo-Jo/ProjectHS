
using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;

////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class MainLobbyUpgradeView : MonoBehaviour
    {
        [Inject] private MainLobbyUpgradeViewModel _viewModel;

        private LobbyUpgradeSlot[] _slots;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // slot 검색
           _slots = GetComponentsInChildren<LobbyUpgradeSlot>();
            // 버튼 초기화
            _viewModel.OnDataChanged += UpdateUI;

            UpdateUI();
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
        private async void UpdateUI() {
            await UniTask.SwitchToMainThread(); // 메인쓰레드로 전환

            foreach (var slot in _slots) {
                // Data
                UpgradeType upgradeType = slot.GetUpgradeType();

                int price = _viewModel.GetPrice(upgradeType);
                int abliltyValue = _viewModel.GetAbilityValue(upgradeType);
                
                // UI 수정
                slot.SetValueText((abliltyValue).ToString()); // Text 변경
                slot.SetPriceText((price).ToString());
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
