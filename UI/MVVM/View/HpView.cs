
using UnityEngine;
using Zenject;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine.Assertions;
using DG.Tweening;
using R3;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class HpView : MonoBehaviour
    {
        [Inject] private HpViewModel _viewModel;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TextMeshProUGUI _text;
        private int _preHp; // Animation 용 이전 HP
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // UI Bind
            _viewModel.RO_MaxHPObservable
                .ThrottleLastFrame(1)
                .Subscribe(UpdateMaxHp)
                .AddTo(this);
            
            _viewModel.RO_CurHpObservable
                .ThrottleLastFrame(1)
                .Subscribe(UpdateHpUI)
                .AddTo(this);


            _viewModel.Notify();
        }

        private void OnDestroy() {
            DOTween.Kill(this.transform);
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

        private void UpdateHpUI(int curHp) {
            int maxHp = _viewModel.RO_MaxHPObservable.CurrentValue;
            _text.text = $"{(curHp > 0 ? curHp : 0)}/{(maxHp > 0 ? maxHp : 0)}";
            _fillImage.fillAmount = maxHp > 0 ? (float)curHp / maxHp : 0f;
            HpAnimation(curHp);
        }

        private void UpdateMaxHp(int value) {
            // 추후 에니메이션 추가
        }

        private void HpAnimation(int curHp) {
            if (_preHp < curHp) { // 증가


            } else if (_preHp > curHp) { // 감소
                this.transform.DOShakePosition(0.2f, 3f);

            }
            _preHp = curHp;
        }

       

    }
}
