
using Zenject;
using System;
using Data;
using R3;
using Domain;
namespace UI
{
    public class HpViewModel {
        [Inject] private HpModel _model;   
        
        public ReadOnlyReactiveProperty<int> RO_CurHpObservable => _model.RO_CurHpObservable;
        public ReadOnlyReactiveProperty<int> RO_MaxHPObservable => _model.RO_MaxHpObservable;

        /// <summary>
        /// 갱신 알림
        /// </summary>
        public void Notify() => _model.Notify();



    }
} 
