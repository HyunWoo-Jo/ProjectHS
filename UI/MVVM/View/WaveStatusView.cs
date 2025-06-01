
using UnityEngine;
using Zenject;
using System;
using UnityEngine.UI;
using TMPro;
using ModestTree;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
namespace UI
{
    public class WaveStatusView : MonoBehaviour
    {
        [Inject] private WaveStatusViewModel _viewModel;

        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _waveTimeText;

        private void Awake() {
#if UNITY_EDITOR // Assertion
            RefAssert();
#endif
            // 버튼 초기화
            _viewModel.OnWaveLevelChanged += UpdateWaveLevel;
            _viewModel.OnTimeChanged += UpdateTime;
        }

        private void OnDestroy() {
            _viewModel.OnWaveLevelChanged -= UpdateWaveLevel;
            _viewModel.OnTimeChanged -= UpdateTime;
            _viewModel = null; // 참조 해제
        }

#if UNITY_EDITOR
        // 검증
        private void RefAssert() {
            Assert.IsNotNull(_waveText);
            Assert.IsNotNull(_waveTimeText);
        }
#endif
        // UI 갱신
      
        private void UpdateWaveLevel(int level) {
            _waveText.text = "WAVE " + level.ToString();
        }
        private void UpdateTime(float time) {
            string s = TimeSpan.FromSeconds(time)
            .ToString(@"mm\:ss");
            _waveTimeText.text = s;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // your logic here

    }
}
