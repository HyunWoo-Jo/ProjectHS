using UnityEngine;
using Contracts;
using Zenject;
using Data;
using Domain;
namespace GamePlay
{
    public class UpgradeService : IUpgradeService 
    {
        [Inject] private SelectedUpgradeModel _selectedUpgradeModel;
        [Inject] private IUpgradeSystem _upgradeSystem;

        public void Reroll(int index) {
            Debug.Log("Rerl");
            if (_selectedUpgradeModel.RerollCount > 0) {
                var list = _upgradeSystem.GetRandomUpgradeDataList(1);
                if (list.Count > 0) {
                    _selectedUpgradeModel.SetUpgradeData(index, list[0]);
                }
                // model 업데이트
                _selectedUpgradeModel.UseRerollCount();
            }
        }

        public void ApplyUpgrade(int index) {
            _selectedUpgradeModel.GetRO_UpgradeData(index).CurrentValue.ApplyUpgrade();

            // 업그레이드 소모
            _upgradeSystem.ConsumeRemainingCount();
            // 남은 업그레이드가 있으면 재출력 시도
            _upgradeSystem.TryShowRemainUpgradeSelection();
        }
    }
}
