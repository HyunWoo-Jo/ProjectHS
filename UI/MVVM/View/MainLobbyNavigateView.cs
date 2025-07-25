
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using System;
using CustomUtility;
using Data;
using Zenject;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using R3;
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

            _viewModel.RO_CurrentActivePanelObservable
                .Subscribe(UpdateButtonAndPanelUI)
                .AddTo(this);
            InitButton();

        }

        private void OnDestroy() {
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
        private void UpdateButtonAndPanelUI(MainLobbyNavigateViewModel.PanelType currentActivePanel) {
            
            if (currentActivePanel != _viewModel.PreActivePanel) { // 같은 패널이 아닐경우 에만 실행
                // 패널 위치 조정
                float targetPanelPosX = (int)currentActivePanel * _mainCanvas.GetRectTransform().sizeDelta.x; // canvas x 사이즈에 맞춰 타겟 위치 설정
                _mainPanel.DOLocalMoveX(-targetPanelPosX, _style.panelMoveDuration).SetEase(_style.panelMoveEase); // 패널 이동 애니메이션

                // 버튼 애니메이션
                EventTrigger preButton = GetButtonPanleType(_viewModel.PreActivePanel);
                ButtionAnimation(preButton, _style.buttonOriginalSize, _style.buttonAnimDuration); // 이전 버튼을 작아지게

                EventTrigger currentButton = GetButtonPanleType(currentActivePanel);
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
            _worldButton.ToObservableEventTrigger(className, methodName + "_world")
                .OnPointerDownAsObservable()
                .ThrottleFirstFrame(1)
                .Subscribe(_ => _viewModel.OnClickPanelMoveButton(MainLobbyNavigateViewModel.PanelType.World))
                .AddTo(this);

            _upgradeButton.ToObservableEventTrigger(className, methodName + "_upgrade")
               .OnPointerDownAsObservable()
               .ThrottleFirstFrame(1)
               .Subscribe(_ => _viewModel.OnClickPanelMoveButton(MainLobbyNavigateViewModel.PanelType.Upgrade))
               .AddTo(this);

            _playButton.ToObservableEventTrigger(className, methodName + "_playButton")
               .OnPointerDownAsObservable()
               .Take(1)
               .Subscribe(_ => OnClickPlayButton())
               .AddTo(this);
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
