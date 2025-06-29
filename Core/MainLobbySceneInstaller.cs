using Contracts;
using GamePlay;
using UI;
using UnityEngine;
using Zenject;

namespace Core
{
    /// <summary>
    /// Main Lobby Scene�� Bind�� �ϴ� Ŭ����
    /// </summary>
    public class MainLobbySceneInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            // UI ViewModel�� Bind
            Container.Bind<MainLobbyNavigateViewModel>().AsTransient();
            Container.BindInterfacesAndSelfTo<MainLobbyUpgradeViewModel>().AsCached();

            // Service;
            Container.Bind<IGlobalUpgradePurchaseService>().To<GlobalUpgradePurchaseService>().AsCached();
           
        }
    }
}
