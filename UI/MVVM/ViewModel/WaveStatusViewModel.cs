
using Zenject;
using System;
using Data;
using System.Diagnostics;
using R3;
namespace UI
{
    public class WaveStatusViewModel
    {
        [Inject] private WaveStatusModel _model;

        public ReadOnlyReactiveProperty<int> RO_WaveLevelObservable => _model.waveLevelObservable;
        public ReadOnlyReactiveProperty<float> RO_WaveTimeObservable => _model.waveTimeObservable;

        public void Notify() {
            _model.waveTimeObservable.ForceNotify();
            _model.waveLevelObservable.ForceNotify();
        }

    }
} 
