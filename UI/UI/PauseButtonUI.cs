using CustomUtility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Zenject;

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

            // ��ư �ʱ�ȭ
            _pauseButton.AddTrigger(EventTriggerType.PointerDown, OnInstancePauseUI, GetType().Name, nameof(OnInstancePauseUI));
        }

        private void OnInstancePauseUI() {
            _uiFactory.InstanceUI<PausePanelView>(91);
        }

    }
}
