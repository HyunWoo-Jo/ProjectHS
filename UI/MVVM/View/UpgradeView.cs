
using UnityEngine;
using Zenject;
using Data;
using CustomUtility;
using TMPro;
using ModestTree;
using R3;
using System;
using Contracts;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI {
    public class UpgradeView : MonoBehaviour
    {
        [Inject] private RarityColorStyleSO _style;
        [Inject] private UpgradeViewModel _viewModel;

        [SerializeField] private TextMeshProUGUI _rerollText;
        private UpgradeCard_UI[] _cards;
        private bool isClick;

        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            _cards = GetComponentsInChildren<UpgradeCard_UI>();

            Bind();
            ButtonInit();

            // UI 갱신
            _viewModel.Notify();
        }
        

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_rerollText);
        }
#endif


        /// <summary>
        /// Bind
        /// </summary>
        private void Bind() {
            for (int i = 0; i < _cards.Length; i++) {
                int capturedIndex = i;
                _viewModel.GetRO_UpgradeDataObservable(i)
                    .ThrottleLastFrame(1)
                    .Subscribe(data => UpdateUI(capturedIndex, data))
                    .AddTo(this);
            }
            _viewModel.RO_RerollCountObservable
              .Subscribe(UpdateRerollUI)
              .AddTo(this);
        }

        /// <summary>
        /// 버튼 초기화
        /// </summary>
        private void ButtonInit() {
            string className = GetType().Name;
            for (int i = 0; i < _cards.Length; i++) {
                int capturedIndex = i;
                // 버튼 초기화
                _cards[i].Button.ToObservableEventTrigger(className, nameof(Selecte))
                    .OnPointerClickAsObservable()
                    .Take(1)
                    .Subscribe(_ => Selecte(capturedIndex))
                    .AddTo(this);

                // 리롤 버튼 초기화
                _cards[i].RerollButton.ToObservableEventTrigger(className, nameof(Reroll))
                    .OnPointerClickAsObservable()
                    .ThrottleFirst(TimeSpan.FromSeconds(1))
                    .Subscribe(_ => Reroll(capturedIndex))
                    .AddTo(this);

            }
        }

        // UI 갱신
        private void UpdateUI(int index, IUpgradeData upgradeData) {
            UpgradeCard_UI card = _cards[index];
            if (upgradeData != null) {
                card.SetSprite(upgradeData.Sprite());
                card.SetName(upgradeData.UpgradeName());
                card.SetNameColor(_style.GetColor((Rarity)upgradeData.Rarity()));

                card.SetDescription(upgradeData.Description());
                card.gameObject.SetActive(true);

            } else {
                card.gameObject.SetActive(false);
            }
        }

        private void UpdateRerollUI(int count) {
            int rerollCount = count;
            bool isActive = count > 0;
            foreach (var card in _cards) {
                card.SetActiveReroll(isActive);
            }
            _rerollText.text = rerollCount.ToString();
        }

        private void Reroll(int index) {
            _viewModel.Reroll(index);
        }


        private void Selecte(int index) {
            if (isClick) return;
            _viewModel.SelectUpgrade(index);
            isClick = true;
            Destroy(this.gameObject); // 선택 후 제거 등 로직
        }
        
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
