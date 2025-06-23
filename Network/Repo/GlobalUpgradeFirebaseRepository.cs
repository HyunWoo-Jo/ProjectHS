using UnityEngine;
using Data;
using Zenject;
using System;
using CustomUtility;
using Cysharp.Threading.Tasks;
namespace Network
{

    /// <summary>
    /// 이후 Firebase 로직으로 변경
    /// </summary>
    public class GlobalUpgradeFirebaseRepository : IGlobalUpgradeRepository
    {
        private UpgradeModel _model;
        [Inject] private IUpgradeService _upgradeService;
        [Inject] private GlobalUpgradeDataSO _tableSO;
        private event Action _OnValueChanged;

        public GlobalUpgradeFirebaseRepository() {
            _model = new UpgradeModel();
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        public async UniTask LoadValue() {
            // 업그레이드 단계를 불러옴 user 정보
            await _upgradeService.GetAllUpgradeTableAsync(_tableSO);

            await _upgradeService.GetAllUpgradeLevelAsync((data) => {  
                _model.SetNewData(data);
            });

            _OnValueChanged?.Invoke();

        }

        public GlobalUpgradeDataSO GetUpgradeTable() {
            return _tableSO;
        }

        public void SetValue(UpgradeType type, int value) {
            _upgradeService.SetUpgradeAsync(type.ToString(), value); // 네트워크 업데이트 요청
            _model.SetValue(type, value); // 모델 업데이트
            _OnValueChanged?.Invoke();
        }

        // 업그레이드 단계를 가지고옴
        public int GetValueLocal(UpgradeType type) {
            return _model.GetValue(type); // 로컬에서 가지고옴   
        }

        public void AddChangedHandler(Action handler) {
            _OnValueChanged += handler;
        }

        public void RemoveChangedHandler(Action handler) {
            _OnValueChanged -= handler;
        }
    }
}
