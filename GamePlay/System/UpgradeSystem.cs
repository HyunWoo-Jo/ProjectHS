using UnityEngine;
using Data;
using Zenject;
namespace GamePlay
{
    public class UpgradeSystem : MonoBehaviour 
    {
        [Inject] private GameDataHub _gameDataHub;
        [Inject] private IGlobalUpgradeRepository _globalUpgradeRepository;
        [Inject] private UpgradeModel _upgradeModel;

        private void Upgrade() {

        }


        
    }
}
