
using Zenject;
using System;
using Domain;
using System.Diagnostics;
using R3;
namespace UI
{
    public class WaveStatusViewModel
    {
        [Inject] private WaveStatusModel _model;

        public ReadOnlyReactiveProperty<int> RO_WaveLevelObservable => _model.RO_WaveLevelObservable;
        public ReadOnlyReactiveProperty<float> RO_WaveTimeObservable => _model.RO_WaveTimeObservable;

        public void Notify() {
            _model.Notify();
        }

    }
} 
