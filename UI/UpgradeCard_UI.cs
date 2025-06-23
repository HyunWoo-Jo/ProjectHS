using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeCard_UI : MonoBehaviour
    {
        [SerializeField] private EventTrigger _button; // 버튼
        [SerializeField] private Image _image; // 카드 이미지
        [SerializeField] private TextMeshProUGUI _nameText; // 이름
        [SerializeField] private TextMeshProUGUI _descriptionText; // 설명

        // Slot Data

        private void Awake() {
#if UNITY_EDITOR
            // 검증
            Assert.IsNotNull(_button);
            Assert.IsNotNull(_image);
            Assert.IsNotNull(_nameText);
            Assert.IsNotNull(_descriptionText);
#endif 
        }

    }
}
