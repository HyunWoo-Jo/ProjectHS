
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
using R3;
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
            // UI Bind
            _viewModel.OnDataChanged += UpdatCrystalUI;

            // 버튼 초기화
            _enterButton.ToObservableEventTrigger(GetType().Name, nameof(OnClickButton))
                .OnPointerDownAsObservable()
                .Take(1)
                .Subscribe(_ => OnClickButton())
                .AddTo(this);

            
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


        private void OnClickButton() {
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
