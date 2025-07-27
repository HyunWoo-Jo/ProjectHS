
using Zenject;
using System;
using Data;
using Contracts;
using System.Collections.Generic;
using System.Diagnostics;
using R3;
using Domain;
namespace UI
{
    public class UpgradeViewModel
    {
        [Inject] private SelectedUpgradeModel _model;
        [Inject] private IUpgradeService _upgradeService;

     
        public ReadOnlyReactiveProperty<int> RO_RerollCountObservable => _model.GetRO_RerollCount;


        public ReadOnlyReactiveProperty<IUpgradeData> GetRO_UpgradeDataObservable(int index) => _model.GetRO_UpgradeData(index);

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
        public void Notify() => _model.Notify();

    }
} 
