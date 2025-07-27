
using UnityEngine;
using Zenject;
using System;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Assertions;
using CustomUtility;
using Data;
using R3;
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
        private bool _isAction = false; // button 기능 작동중인가 체크
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            string name = GetType().Name;

            _giveUpButton.ToObservableEventTrigger(name, nameof(OnGiveUpButton))
                .OnPointerClickAsObservable()
                .Take(1)
                .Subscribe(OnGiveUpButton)
                .AddTo(this);

            _returnButton.ToObservableEventTrigger(name, nameof(OnReturnButton))
                .OnPointerClickAsObservable()
                .Take(1)
                .Subscribe(OnReturnButton)
                .AddTo(this);
        }


        private void OnEnable() {
            _isAction = false;
            GameSettings.IsPause = true;
            // UI 갱신
            UpdateWaveTextUI(_viewModel.Level);
        }

        private void OnDisable() {
            _isAction = false;
            GameSettings.IsPause = false;
        }


#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_giveUpButton);
            Assert.IsNotNull(_returnButton);
            Assert.IsNotNull(_waveText);
        }
#endif

////////////////////////////////////////////////////////////////////////////////////
        // your logic here

        private void OnGiveUpButton(PointerEventData data) {
            if (_isAction) return;
            _isAction = true;
            _viewModel.ChangeScene();
        }

        private void OnReturnButton(PointerEventData data) {
            if (_isAction) return;
            Destroy(this.gameObject);
        }

        // UI 갱신
        private void UpdateWaveTextUI(int level) {
            _waveText.text = level.ToString();
        }

    }
}
