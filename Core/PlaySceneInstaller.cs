using UnityEngine;
using Zenject;
using Data;
using GamePlay;
using UI;
namespace Core
{
    public class PlaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<GameDataHub>().AsCached().NonLazy();
            Container.Bind<GameObjectPoolManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();


            // Settings
            Container.Bind<StageSettingsModel>().AsCached();


            // Play Scene에서만 사용되는  View Model
            Container.BindInterfacesAndSelfTo<PurchaseTowerViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<WaveStatusViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<GoldViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<ExpViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<HpViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<PausePanelViewModel>().AsCached();

            // Play Scene에서만 사용되는  Model
            Container.Bind<PurchaseTowerModel>().AsCached();
            Container.Bind<WaveStatusModel>().AsCached(); // Wave 정보
            Container.Bind<GoldModel>().AsCached();
            Container.Bind<ExpModel>().AsCached();
            Container.Bind<HpModel>().AsCached();

            // Policy
            Container.Bind<GoldPolicy>().AsCached();
            Container.Bind<HpPolicy>().AsCached();
            Container.Bind<ExpPolicy>().AsCached();
        }
    }
}
