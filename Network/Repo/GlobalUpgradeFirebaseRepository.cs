using UnityEngine;
using Data;
using Zenject;
using System;
using CustomUtility;
using Cysharp.Threading.Tasks;
namespace Network
{

    /// <summary>
    /// ���� Firebase �������� ����
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
        /// ������ �ε�
        /// </summary>
        public async UniTask LoadValue() {
            // ���׷��̵� �ܰ踦 �ҷ��� user ����
            await _upgradeService.GetAllUpgradeTableAsync(_tableSO);

            // Upgrade Level�� �о��
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
            _upgradeService.SetUpgradeAsync(type.ToString(), value); // ��Ʈ��ũ ������Ʈ ��û
            _model.SetValue(type, value); // �� ������Ʈ
            _OnValueChanged?.Invoke();
        }

        // ���׷��̵� �ܰ踦 �������
        public int GetLevelLocal(GlobalUpgradeType type) {
            return _model.GetValue(type); // ���ÿ��� �������   
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
