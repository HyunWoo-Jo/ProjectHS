
using Zenject;
using System;
using Data;
using R3;
using Domain;
namespace UI
{
    public class ExpViewModel  {
        [Inject] private ExpModel _model;

        public ReadOnlyReactiveProperty<int> RO_LevelObservable => _model.RO_LevelObservable;
        public ReadOnlyReactiveProperty<float> RO_CurExpObservable => _model.RO_CurExpObservable;
        public ReadOnlyReactiveProperty<float> RO_NextExpObservable => _model.RO_NextExpObservable;

        public float ExpRatio => _model.ExpRatio;
        // View 에서 로딩이 되면 호출 되는 
        public void Notify() => _model.Notify();
       
    }
} 
