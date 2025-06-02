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
            Container.Bind<StageSettingsModel>().AsCached().NonLazy();


            // Play Scene에서만 사용되는  View Model
            Container.Bind<PurchaseTowerViewModel>().AsTransient().NonLazy();
            Container.BindInterfacesAndSelfTo<WaveStatusViewModel>().AsCached().NonLazy();
            // Play Scene에서만 사용되는  Model
            Container.Bind<WaveStatusModel>().AsCached().NonLazy(); // Wave 정보
        }
    }
}
