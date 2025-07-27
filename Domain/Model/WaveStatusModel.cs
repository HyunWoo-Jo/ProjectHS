using UnityEngine;
using CustomUtility;
using R3;
using log4net.Core;
namespace Domain
{
    public sealed class WaveStatusModel
    {
        private ReactiveProperty<int> _waveLevelObservable { get; } = new(0); // 스테이지 레벨
        private ReactiveProperty<float> _waveTimeObservable { get; } = new(0); // 웨이브 타임


        public ReadOnlyReactiveProperty<int> RO_WaveLevelObservable => _waveLevelObservable;
        public ReadOnlyReactiveProperty<float> RO_WaveTimeObservable => _waveTimeObservable;

        public int BaseWaveTime { get; private set; } = 20;

        public int WaveLevel {
            get { return _waveLevelObservable.Value; }
            private set { _waveLevelObservable.Value = value; }
        }

        public float WaveTime {
            get { return _waveTimeObservable.Value; }
            private set { _waveTimeObservable.Value = value; }
        }

        /// <summary> 웨이브 레벨을 강제 설정 </summary>
        public void SetWaveLevel(int level) => WaveLevel = level;

        /// <summary> 웨이브 시간 감소 처리 및 웨이브 진행 </summary>
        public void ConsumeWaveTime(float deltaTime) {
            WaveTime -= deltaTime;

            while (WaveTime <= 0) {
                WaveLevel++;
                WaveTime += BaseWaveTime; // 다음 웨이브 시간 추가
            }
        }
        public void Notify() {
            _waveLevelObservable.ForceNotify();
            _waveTimeObservable.ForceNotify();
        }
    }
}
