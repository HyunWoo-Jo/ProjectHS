using Contracts;
using GamePlay;
using UI;
using UnityEngine;
using Zenject;

namespace Core
{
    /// <summary>
    /// Main Lobby Scene의 Bind를 하는 클레스
    /// </summary>
    public class MainLobbySceneInstaller : MonoInstaller
    {
        public override void InstallBindings() {

            Container.Bind<MainLobbyNavigateViewModel>().AsCached();
            Container.Bind<MainLobbyUpgradeViewModel>().AsCached();

            // Service;
            Container.Bind<IGlobalUpgradePurchaseService>().To<GlobalUpgradePurchaseService>().AsCached();
            

           
        }
    }
}
