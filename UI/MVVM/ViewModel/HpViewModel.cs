
using Zenject;
using System;
using Data;
using R3;
namespace UI
{
    public class HpViewModel {
        [Inject] private HpModel _hpModel;   
        
        public ReadOnlyReactiveProperty<int> RO_CurHpObservable => _hpModel.curHpObservable;
        public ReadOnlyReactiveProperty<int> RO_MaxHPObservable => _hpModel.maxHpObservable;

        /// <summary>
        /// 갱신 알림
        /// </summary>
        public void Notify() {
            _hpModel.curHpObservable.ForceNotify();
            _hpModel.maxHpObservable.ForceNotify();
        }



    }
} 
