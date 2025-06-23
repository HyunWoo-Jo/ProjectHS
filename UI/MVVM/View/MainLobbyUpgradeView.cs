
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

            GlobalUpgradeDataSO table = _viewModel.GetUpgradeDataSO();
            foreach (var slot in _slots) {
                UpgradeType upgradeType = slot.GetUpgradeType();
                int upgradeLevel = _viewModel.GetData(upgradeType);
                slot.SetValueText(upgradeLevel.ToString()); // level을 가지고옴

                // Data

                int priceIncresment = table.GetPrice(upgradeType);
                // 각 type에 맞는 정보를 table을 통해 가지고옴
               
                slot.SetPriceText((upgradeLevel * priceIncresment).ToString());
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
