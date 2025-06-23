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
        private UpgradeModel _model;
        [Inject] private IUpgradeService _upgradeService;
        [Inject] private GlobalUpgradeDataSO _tableSO;
        private event Action _OnValueChanged;

        public GlobalUpgradeFirebaseRepository() {
            _model = new UpgradeModel();
        }

        /// <summary>
        /// ������ �ε�
        /// </summary>
        public async UniTask LoadValue() {
            // ���׷��̵� �ܰ踦 �ҷ��� user ����
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
            _upgradeService.SetUpgradeAsync(type.ToString(), value); // ��Ʈ��ũ ������Ʈ ��û
            _model.SetValue(type, value); // �� ������Ʈ
            _OnValueChanged?.Invoke();
        }

        // ���׷��̵� �ܰ踦 �������
        public int GetValueLocal(UpgradeType type) {
            return _model.GetValue(type); // ���ÿ��� �������   
        }

        public void AddChangedHandler(Action handler) {
            _OnValueChanged += handler;
        }

        public void RemoveChangedHandler(Action handler) {
            _OnValueChanged -= handler;
        }
    }
}
