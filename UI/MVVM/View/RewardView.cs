
using UnityEngine;
using Zenject;
using System;
using Data;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using CustomUtility;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Profiling.HierarchyFrameDataView;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class RewardView : MonoBehaviour
    {
        [Inject] private RewardViewModel _viewModel;
        [SerializeField] private TextMeshProUGUI _crystalText;
        [SerializeField] private EventTrigger _enterButton;
        private bool _isMove = false;   
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnDataChanged += UpdatCrystalUI;
            InitButton();
            UpdatCrystalUI(_viewModel.RewardCrystal);
        }

        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdatCrystalUI;
            _viewModel = null; // 참조 해제
        }

        private void OnEnable() {
            GameSettings.IsPause = true;
        }

        private void OnDisable() {
            GameSettings.IsPause = false;
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_crystalText);
            Assert.IsNotNull(_enterButton);
        }
#endif
        // 버튼 초기화
        private void InitButton() {
            _enterButton.AddTrigger(EventTriggerType.PointerClick, EnterButton, GetType().Name, nameof(EnterButton));
        }

        private void EnterButton() {
            if (_isMove) return;
            _viewModel.ChangeScene();
        }

        // UI 갱신
        private void UpdatCrystalUI(int reward) {
            _crystalText.text = reward.ToString();
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
