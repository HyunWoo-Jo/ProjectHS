using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace UI
{
    public class LobbyUpgradeSlot : MonoBehaviour
    {
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private EventTrigger _buyButton;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private TextMeshProUGUI _priceText;
        
        public UpgradeType GetUpgradeType() => _upgradeType;
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_valueText);
            Assert.IsNotNull(_priceText);
#endif
        }

        public void SetValueText(string value) { _valueText.text = value; }
        public void SetPriceText(string priceText) { _priceText.text = priceText; }
    }
}
