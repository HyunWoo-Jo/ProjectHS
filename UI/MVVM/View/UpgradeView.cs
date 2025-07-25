
using UnityEngine;
using Zenject;
using Data;
using CustomUtility;
using TMPro;
using ModestTree;
using R3;
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
            
            // UI 초기화
            for(int i = 0; i < _cards.Length; i++) {
                int capturedIndex = i;
                _viewModel.GetRO_UpgradeDataObservable(i)
                    .Subscribe(data => UpdateUI(capturedIndex, data))
                    .AddTo(this);
            }
            _viewModel.RO_RerollCountObservable
                .Subscribe(UpdateRerollUI)
                .AddTo(this);

            // 버튼 초기화
            string className = GetType().Name;
            for (int i = 0; i < _cards.Length; i++) {
                int index = i;
                // 버튼 초기화
                _cards[i].Button.AddTrigger(
                    UnityEngine.EventSystems.EventTriggerType.PointerClick,
                    () => {
                        Selecte(index);
                    },
                    className,
                    nameof(Selecte)
                );
                // 리롤 버튼 초기화
                _cards[i].RerollButton.AddTrigger(
                    UnityEngine.EventSystems.EventTriggerType.PointerClick,
                    () => {
                        Reroll(index);
                    },
                    className, 
                    nameof(Reroll)
                );
            }
            // UI 갱신
            _viewModel.Notify();
        }
        

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_rerollText);
        }
#endif
        // UI 갱신
        private void UpdateUI(int index, UpgradeDataSO updateData) {
            UpgradeDataSO upgradeData = updateData;
            UpgradeCard_UI card = _cards[index];
            if (upgradeData != null) {
                card.SetSprite(upgradeData.sprite);
                card.SetName(upgradeData.upgradeName);
                card.SetNameColor(_style.GetColor(upgradeData.rarity));

                card.SetDescription(upgradeData.description);
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
