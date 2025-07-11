
using UnityEngine;
using Zenject;
using Data;
using CustomUtility;
using TMPro;
using ModestTree;
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
            
            // 버튼 초기화
            _viewModel.OnDataChanged += UpdateUI;

            _viewModel.OnRerollCountChanged += UpdateRerollUI;

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

                UpdateUI(i); // card 개수 만큼 UI 초기화 호출
            }
            UpdateRerollUI(_viewModel.RerollCount);
        }

        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdateUI;
            _viewModel.OnRerollCountChanged -= UpdateRerollUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_rerollText);
        }
#endif
        // UI 갱신
        private void UpdateUI(int index) {
            if (_cards.Length <= index) return;
            UpgradeDataSO upgradeData = _viewModel.GetUpgradeData(index);
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
