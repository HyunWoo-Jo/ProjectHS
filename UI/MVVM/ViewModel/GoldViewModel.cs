
using Zenject;
using System;
using Data;
using R3;
using Domain;
namespace UI
{
    public class GoldViewModel
    {   
        [Inject] private GoldModel _model;

        public ReadOnlyReactiveProperty<int> RO_GoldObservable => _model.RO_GoldObservable;
       
        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        public void Notify() => _model.Notify();
      

       
    }
} 
