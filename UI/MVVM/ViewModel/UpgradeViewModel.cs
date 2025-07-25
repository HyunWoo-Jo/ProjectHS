
using Zenject;
using System;
using Data;
using Contracts;
using System.Collections.Generic;
using System.Diagnostics;
using R3;
namespace UI
{
    public class UpgradeViewModel
    {
        [Inject] private SelectedUpgradeModel _model;
        [Inject] private IUpgradeService _upgradeService;

     
        public ReadOnlyReactiveProperty<int> RO_RerollCountObservable => _model.rerollCountObservable;


        public ReadOnlyReactiveProperty<UpgradeDataSO> GetRO_UpgradeDataObservable(int index) {
            return _model.upgradeDatasObservable[index];
        }

        /// <summary>
        /// 업그레이드 선택
        /// </summary>
        public void SelectUpgrade(int index) {
            _upgradeService.ApplyUpgrade(index); // 업그레이드 적용
        }

        /// <summary>
        /// 리롤 요청
        /// </summary>
        public void Reroll(int index) {
            _upgradeService.Reroll(index);
        }

        /// <summary>
        /// UI 갱신
        /// </summary>
        public void Notify() {
            foreach (var dataObservable in _model.upgradeDatasObservable) {
                dataObservable.ForceNotify();
            }
            _model.rerollCountObservable.ForceNotify();
        }

    }
} 
