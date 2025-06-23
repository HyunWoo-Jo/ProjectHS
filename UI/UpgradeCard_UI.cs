using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeCard_UI : MonoBehaviour
    {
        [SerializeField] private EventTrigger _button; // ��ư
        [SerializeField] private Image _image; // ī�� �̹���
        [SerializeField] private TextMeshProUGUI _nameText; // �̸�
        [SerializeField] private TextMeshProUGUI _descriptionText; // ����

        // Slot Data

        private void Awake() {
#if UNITY_EDITOR
            // ����
            Assert.IsNotNull(_button);
            Assert.IsNotNull(_image);
            Assert.IsNotNull(_nameText);
            Assert.IsNotNull(_descriptionText);
#endif 
        }

    }
}
