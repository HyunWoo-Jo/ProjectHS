
using UnityEngine;
using TMPro;
using Zenject;
using ModestTree;
using static UnityEditor.Profiling.HierarchyFrameDataView;
using Cysharp.Threading.Tasks;

////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class CrystalView : MonoBehaviour
    {
        [Inject] private CrystalViewModel _viewModel;

        [SerializeField] private TextMeshProUGUI _text;
        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
         
            _viewModel.OnDataChanged += UpdateUI;
        }

        private void Start() {
            _viewModel.Update();
        }

        private void OnDestroy() {
            _viewModel.OnDataChanged -= UpdateUI;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_text);
        }
#endif
        // UI 갱신
        private async void UpdateUI(int value) {
            await UniTask.SwitchToMainThread(); // 메인쓰레드로 전환
            _text.text = value.ToString();
        }
////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
