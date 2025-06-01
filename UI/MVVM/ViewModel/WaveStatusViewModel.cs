
using Zenject;
using System;
using Data;
using System.Diagnostics;
namespace UI
{
    public class WaveStatusViewModel : IInitializable, IDisposable
    {
        [Inject] private WaveStatusModel _model;

        public event Action<int> OnWaveLevelChanged; // 데이터가 변경될떄 호출될 액션 (상황에 맞게 변수명을 변경해서 사용)
        public event Action<float> OnTimeChanged;


        // Zenject에서 관리
        public void Initialize() {
            // Model에 자동 Bind 되도록 설정
            _model.waveLevelObservable.OnValueChanged += UpdateWaveLevel;
            _model.waveTimeObservable.OnValueChanged += UpdateWaveTime;

            _model.waveLevelObservable.Value = 0;
            _model.waveTimeObservable.Value = 0;
        }

        // Zenject에서 관리
        public void Dispose() {
            _model.waveLevelObservable.OnValueChanged -= UpdateWaveLevel;
            _model.waveTimeObservable.OnValueChanged -= UpdateWaveTime;
        }

        private void UpdateWaveLevel(int level) {
            OnWaveLevelChanged?.Invoke(level);
        }
        private void UpdateWaveTime(float time) {
            OnTimeChanged?.Invoke(time);
        }


    }
} 
