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
            // model 업데이트
            _selectedUpgradeModel.observableRerollCount.Value -= 1;
        }

        public void ApplyUpgrade(int index) {
            _selectedUpgradeModel.observableUpgradeDatas[index].Value.ApplyUpgrade();

            // 업그레이드 소모
            _upgradeSystem.ConsumeRemainingCount();
            // 남은 업그레이드가 있으면 재출력 시도
            _upgradeSystem.TryShowRemainUpgradeSelection();
        }
    }
}
