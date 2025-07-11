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
        [SerializeField] private EventTrigger _button; // ��ư
        [SerializeField] private EventTrigger _rerollButton; // ���� ��ư
        [SerializeField] private Image _image; // ī�� �̹���
        [SerializeField] private TextMeshProUGUI _nameText; // �̸�
        [SerializeField] private TextMeshProUGUI _descriptionText; // ����

        // Slot Data

        private void Awake() {
#if UNITY_EDITOR
            // ����
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
