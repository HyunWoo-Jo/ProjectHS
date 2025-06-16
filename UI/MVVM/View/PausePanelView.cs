
using UnityEngine;
using Zenject;
using System;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Assertions;
using CustomUtility;
using Data;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class PausePanelView : MonoBehaviour
    {
        [Inject] private PausePanelViewModel _viewModel;
        [SerializeField] private EventTrigger _giveUpButton;
        [SerializeField] private EventTrigger _returnButton;
        [SerializeField] private TextMeshProUGUI _waveText;
        [Inject] public IUIFactory _uiManager; // UI effect 생성용 Interface
        private bool _isAction = false; // button 기능 작동중인가 체크
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnLevelEvent += UpdateWaveTextUI;
            ButtonInit();
        }


        private void OnEnable() {
            _isAction = false;
            GameSettings.IsPause = true;
            _viewModel.UpdatePanel(); // 패널 갱신 요청
        }

        private void OnDisable() {
            _isAction = false;
            GameSettings.IsPause = false;
        }
        private void OnDestroy() {
            _viewModel.OnLevelEvent -= UpdateWaveTextUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_giveUpButton);
            Assert.IsNotNull(_returnButton);
            Assert.IsNotNull(_waveText);
        }
#endif
        // UI 갱신
        private void UpdateUI() {
            
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here
        private void ButtonInit() {
            // 버튼 초기화
            _giveUpButton.AddTrigger(EventTriggerType.PointerClick, OnGiveUpButton, GetType().Name, nameof(OnGiveUpButton));
            _returnButton.AddTrigger(EventTriggerType.PointerClick, OnReturnButton, GetType().Name, nameof(OnReturnButton));
        }

        private void OnGiveUpButton() {
            if (_isAction) return;
            _isAction = true;
            IWipeUI wipeUI = _uiManager.InstanceUI<WipeUI>(20); // UI 생성 요청
            float loadDelay = 0.5f;
            wipeUI.Wipe(WipeDirection.Left, loadDelay, false); // 이펙트 호출
            _viewModel.ChangedScene(loadDelay);
        }

        private void OnReturnButton() {
            if (_isAction) return;
            this.gameObject.SetActive(false);
        }

        // UI 갱신
        private void UpdateWaveTextUI(int level) {
            _waveText.text = level.ToString();
        }

    }
}
