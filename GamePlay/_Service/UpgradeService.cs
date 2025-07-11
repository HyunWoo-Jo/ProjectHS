using UnityEngine;
using Contracts;
using Zenject;
using Data;
namespace GamePlay
{
    public class UpgradeService : IUpgradeService 
    {
        [Inject] private SelectedUpgradeModel _selectedUpgradeModel;
        [Inject] private IUpgradeSystem _upgradeSystem;

        public void Reroll(int index) {
            var list = _upgradeSystem.GetRandomUpgradeDataList(1);
            if (list.Count > 0) {
                _selectedUpgradeModel.observableUpgradeDatas[index].Value = list[0];
            }
            // model ������Ʈ
            _selectedUpgradeModel.observableRerollCount.Value -= 1;
        }

        public void ApplyUpgrade(int index) {
            _selectedUpgradeModel.observableUpgradeDatas[index].Value.ApplyUpgrade();

            // ���׷��̵� �Ҹ�
            _upgradeSystem.ConsumeRemainingCount();
            // ���� ���׷��̵尡 ������ ����� �õ�
            _upgradeSystem.TryShowRemainUpgradeSelection();
        }
    }
}
