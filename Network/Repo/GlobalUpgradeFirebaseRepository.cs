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
        private GlobalUpgradeModel _model;
        [Inject] private IGlobalUpgradeNetworkService _upgradeService;
        [Inject] private GlobalUpgradeDataSO _tableSO;
        private event Action _OnValueChanged;

        public GlobalUpgradeFirebaseRepository() {
            _model = new GlobalUpgradeModel();
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        public async UniTask LoadValue() {
            // 업그레이드 단계를 불러옴 user 정보
            await _upgradeService.GetAllUpgradeTableAsync(_tableSO);

            // Upgrade Level을 읽어옴
            await _upgradeService.GetAllUpgradeLevelAsync((data) => {  
                _model.SetNewData(data);
            });

            _OnValueChanged?.Invoke();

        }

        /// Value
        public int GetPrice(GlobalUpgradeType type) {
            return GetLevelLocal(type) * _tableSO.GetPriceIncrement(type) + _tableSO.GetStartPrice(type); 
        }
        public int GetAbilityValue(GlobalUpgradeType type) {
            return GetLevelLocal(type) * _tableSO.GetValueIncrement(type);
        }


        //// Level
        public void SetLevel(GlobalUpgradeType type, int value) {
            _upgradeService.SetUpgradeAsync(type.ToString(), value); // 네트워크 업데이트 요청
            _model.SetValue(type, value); // 모델 업데이트
            _OnValueChanged?.Invoke();
        }

        // 업그레이드 단계를 가지고옴
        public int GetLevelLocal(GlobalUpgradeType type) {
            return _model.GetValue(type); // 로컬에서 가지고옴   
        }

        //// Listeners
        public void AddChangedListener(Action handler) {
            _OnValueChanged += handler;
        }

        public void RemoveChangedListener(Action handler) {
            _OnValueChanged -= handler;
        }
    }
}
