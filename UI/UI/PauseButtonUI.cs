using CustomUtility;
using R3;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Zenject;
using System;

namespace UI
{
    public class PauseButtonUI : MonoBehaviour
    {
        [SerializeField] private EventTrigger _pauseButton;
        [Inject] private IUIFactory _uiFactory;
       
        void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(_pauseButton);
#endif

            // 버튼 초기화
            _pauseButton.ToObservableEventTrigger(GetType().Name, nameof(OnInstancePauseUI))
                .OnPointerDownAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(1))
                .Subscribe(_ => OnInstancePauseUI())
                .AddTo(this);
        }

        private void OnInstancePauseUI() {
            _uiFactory.InstanceUI<PausePanelView>(91);
        }

    }
}
