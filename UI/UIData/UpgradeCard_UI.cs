using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CustomUtility;
namespace UI
{
    public class UpgradeCard_UI : MonoBehaviour
    {
        [SerializeField] private EventTrigger _button; // 버튼
        [SerializeField] private EventTrigger _rerollButton; // 리롤 버튼
        [SerializeField] private Image _image; // 카드 이미지
        [SerializeField] private TextMeshProUGUI _nameText; // 이름
        [SerializeField] private TextMeshProUGUI _descriptionText; // 설명

        // Slot Data

        private void Awake() {
#if UNITY_EDITOR
            // 검증
            Assert.IsNotNull(_button);
            Assert.IsNotNull(_rerollButton);
            Assert.IsNotNull(_image);
            Assert.IsNotNull(_nameText);
            Assert.IsNotNull(_descriptionText);
#endif 
        }

        public void SetSprite(Sprite sprite) => _image.sprite = sprite;
        public EventTrigger Button => _button;
        public EventTrigger RerollButton => _rerollButton;
        public void SetName(string str) => _nameText.text = str;
        public void SetNameColor(Color color) => _nameText.color = color;
        public void SetDescription(string str) => _descriptionText.text = str;

        public void SetActiveReroll(bool isActive) => _rerollButton.gameObject.SetActive(isActive);
    }
}
