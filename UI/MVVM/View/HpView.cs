
using UnityEngine;
using Zenject;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine.Assertions;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class HpView : MonoBehaviour
    {
        [Inject] private HpViewModel _viewModel;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _text;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnHpChanged += UpdateHpUI;

            _viewModel.Update();
        }

        private void OnDestroy() {
            _viewModel.OnHpChanged -= UpdateHpUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_fillImage);
            Assert.IsNotNull(_text);
        }
#endif

////////////////////////////////////////////////////////////////////////////////////
        // your logic here

        private void UpdateHpUI(int curHp, int maxHp) {
            _text.text = $"{(curHp > 0 ? curHp : 0)}/{(maxHp > 0 ? maxHp : 0)}";
            _fillImage.fillAmount = maxHp > 0 ? (float)curHp / maxHp : 0f;
        }


    }
}
