using Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace UI
{
    public class LobbyUpgradeSlot : MonoBehaviour
    {
        [SerializeField] private GlobalUpgradeType _upgradeType;
        [SerializeField] private EventTrigger _buyButton;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private TextMeshProUGUI _priceText;
        
        public GlobalUpgradeType GetUpgradeType() => _upgradeType;
        
        public EventTrigger GetEventTrigger() => _buyButton;
        
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
