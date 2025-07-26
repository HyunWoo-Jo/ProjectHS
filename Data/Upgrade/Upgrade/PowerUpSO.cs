using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Linq;
namespace Data
{
    [CreateAssetMenu(fileName = "PowerUpSO", menuName = "Scriptable Objects/Upgrade/PowerUp")]
    public class PowerUpSO : UpgradeStrategyBaseSO {
        [Inject] private GameDataHub _dataHub;

        public override void Apply(float value) {
            IEnumerable<TowerData> towerDataEnumer = _dataHub.GetUsedTowerData();


            foreach (var data in towerDataEnumer) {
                data.attackPower += (int)value;
            }
        }
    }
}
