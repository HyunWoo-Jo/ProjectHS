using Data;
using Zenject;
using System;
using System.Diagnostics;
using R3;
using Domain;
namespace UI
{
    public class CrystalViewModel 
    {
        [Inject] private CrystalModel _model; // model

        public ReadOnlyReactiveProperty<int> RO_CrystalObservable => _model.RO_CrystalObservable;



        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        public void Notify() => _model.Notify();
    }
} 
