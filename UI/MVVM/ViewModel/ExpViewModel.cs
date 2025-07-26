
using Zenject;
using System;
using Data;
using R3;
namespace UI
{
    public class ExpViewModel  {
        [Inject] private ExpModel _model;

        public ReadOnlyReactiveProperty<int> RO_LevelObservable => _model.levelObservable;
        public ReadOnlyReactiveProperty<float> RO_CurExpObservable => _model.RO_CurExpObservable;
        public ReadOnlyReactiveProperty<float> RO_NextExpObservable => _model.nextExpObservable;

        public float GetExpRation() {
            return RO_CurExpObservable.CurrentValue / RO_NextExpObservable.CurrentValue;
        }
        // View 에서 로딩이 되면 호출 되는 
        public void Notify() {
            _model.Notify();
        }
    }
} 
