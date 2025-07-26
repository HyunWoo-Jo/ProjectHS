
using Zenject;
using System;
using Data;
using R3;
namespace UI
{
    public class GoldViewModel
    {   
        [Inject] private GoldModel _model;

        public ReadOnlyReactiveProperty<int> RO_GoldObservable => _model.goldObservable;
       
        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        public void Notify() {
            _model.goldObservable.ForceNotify();
        }
      

       
    }
} 
