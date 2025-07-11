
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System;
using CustomUtility;
using Data;
using Zenject;
using DG.Tweening;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class MainLobbyNavigateView : MonoBehaviour
    {
        [SerializeField] private NavigateStyleSettingsSO _style;

        [Inject] private MainLobbyNavigateViewModel _viewModel;

        [Inject] private IMainCanvasTag _mainCanvas;

        [SerializeField] private Transform _mainPanel;
        [Header("Navigate Button")]
        [SerializeField] private EventTrigger _worldButton;
        [SerializeField] private EventTrigger _upgradeButton;

        [Header("World Panel")]
        [SerializeField] private EventTrigger _playButton;


        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화

            _viewModel.OnDataChanged += UpdateButtonAndPanelUI;
            InitButton();

        }

        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdateButtonAndPanelUI;
            _viewModel = null; // 참조 해제

            // Tween 해제
            DOTween.Kill(_mainPanel.gameObject);
            DOTween.Kill(_worldButton.gameObject);
            DOTween.Kill(_upgradeButton.gameObject);
            DOTween.Kill(_playButton.gameObject);
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_style);
            Assert.IsNotNull(_mainPanel);
            Assert.IsNotNull(_worldButton);
            Assert.IsNotNull(_upgradeButton);

            Assert.IsNotNull(_playButton);
        }
#endif
        // UI 갱신 Button / Panel
        private void UpdateButtonAndPanelUI() {
            
            if (_viewModel.CurrentActivePanel != _viewModel.PreActivePanel) { // 같은 패널이 아닐경우 에만 실행
                // 패널 위치 조정
                float targetPanelPosX = (int)_viewModel.CurrentActivePanel * _mainCanvas.GetRectTransform().sizeDelta.x; // canvas x 사이즈에 맞춰 타겟 위치 설정
                _mainPanel.DOLocalMoveX(-targetPanelPosX, _style.panelMoveDuration).SetEase(_style.panelMoveEase); // 패널 이동 애니메이션

                // 버튼 애니메이션
                EventTrigger preButton = GetButtonPanleType(_viewModel.PreActivePanel);
                ButtionAnimation(preButton, _style.buttonOriginalSize, _style.buttonAnimDuration); // 이전 버튼을 작아지게

                EventTrigger currentButton = GetButtonPanleType(_viewModel.CurrentActivePanel);
                ButtionAnimation(currentButton, _style.buttonCloseUpSize, _style.buttonAnimDuration); // 클릭 버튼을 커지게
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////
        // your logic here
        /// <summary>
        /// 버튼 초기화
        /// </summary>
        private void InitButton() {
            string className = GetType().Name;
            string methodName = nameof(InitButton);
            _worldButton.AddTrigger(EventTriggerType.PointerDown,
                new Action(() => _viewModel.OnClickPanelMoveButton(MainLobbyNavigateViewModel.PanelType.World)), // 버튼 기능 바인딩
                className, methodName + "_world");

            _upgradeButton.AddTrigger(EventTriggerType.PointerDown,
                new Action(() => _viewModel.OnClickPanelMoveButton(MainLobbyNavigateViewModel.PanelType.Upgrade)), // 버튼 기능 바인딩
                className, methodName + "_upgrade");

            _playButton.AddTrigger(EventTriggerType.PointerClick, OnClickPlayButton, // 버튼 기능 바인딩
                className, methodName + "_playButton");
        }

        /// <summary>
        /// 버튼의 애니메이션 실행
        /// </summary>
        private void ButtionAnimation(EventTrigger button, Vector2 targetSize, float duration) {
            if (button != null) {
                button.GetComponent<RectTransform>().DOSizeDelta(targetSize, duration).SetEase(Ease.InOutElastic); 
            }
        }
        /// <summary>
        /// 패널에 맞게 버튼 반환
        /// </summary>
        private EventTrigger GetButtonPanleType(MainLobbyNavigateViewModel.PanelType panelType) {
            switch (panelType) {
                case MainLobbyNavigateViewModel.PanelType.World:
                return _worldButton;
                case MainLobbyNavigateViewModel.PanelType.Upgrade:
                return _upgradeButton;
            }
            return null;
        }

        /// <summary>
        /// Play Button 클릭
        /// </summary>
        public void OnClickPlayButton() {
            _viewModel.ChangeScene();
        }
    }
}
