
using UnityEngine;
using TMPro;
using Zenject;

////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class MoneyView : MonoBehaviour
    {
        [Inject] private MoneyViewModel _viewModel;

        [SerializeField] private TextMeshPro _text;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
         
            _viewModel.OnDataChanged += UpdateUI;

        }

        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdateUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {

        }
#endif
        // UI 갱신
        private void UpdateUI() {
            _text.text = _viewModel.GetMoney.ToString();
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
