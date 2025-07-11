
using Zenject;
using System;
using Data;
using Contracts;
using System.Collections.Generic;
using System.Diagnostics;
namespace UI
{
    public class UpgradeViewModel : IInitializable , IDisposable
    {
        [Inject] private SelectedUpgradeModel _model;
        [Inject] private IUpgradeService _upgradeService;

        public event Action<int> OnDataChanged; // 데이터가 변경될떄 호출될 액션 변경된 Model Data index를 넘겨줌
        public event Action<int> OnRerollCountChanged;

        private List<Action<UpgradeDataSO>> _handlerList = new (); // 람다를 저장하는 List
        /// <summary>
        /// 추가 리롤 횟수
        /// </summary>
        public int RerollCount => _model.observableRerollCount.Value;

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

        public UpgradeDataSO GetUpgradeData(int index) {
            if (_model.observableUpgradeDatas.Length <= index) return null;
            return _model.observableUpgradeDatas[index].Value;
        }

        private void RerollCountChanged(int count) {
            OnRerollCountChanged?.Invoke(count);
        }

        // Zenject 에서 관리
        public void Initialize() {
            // Data Changed 구독
            for (int i = 0; i < _model.observableUpgradeDatas.Length; i++) {
                int index = i;
                Action<UpgradeDataSO> handler = (udSO) => { OnDataChanged?.Invoke(index); };
                _model.observableUpgradeDatas[i].OnValueChanged += handler;
                _handlerList.Add(handler);
            }
            // Reroll 구독
            _model.observableRerollCount.OnValueChanged += RerollCountChanged;
        }
        // Zenject 에서 관리
        public void Dispose() {
            // Data Changed 구독
            for (int i = 0; i < _handlerList.Count; i++) {
                _model.observableUpgradeDatas[i].OnValueChanged -= _handlerList[i];
            }
            _handlerList.Clear();
            _model.observableRerollCount.OnValueChanged -= RerollCountChanged;
            // Reroll 구독
        }
    }
} 
