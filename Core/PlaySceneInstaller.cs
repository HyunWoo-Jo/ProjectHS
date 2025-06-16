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


            // Play Scene������ ���Ǵ�  View Model
            Container.BindInterfacesAndSelfTo<PurchaseTowerViewModel>().AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<WaveStatusViewModel>().AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<GoldViewModel>().AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<ExpViewModel>().AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<HpViewModel>().AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<PausePanelViewModel>().AsCached().NonLazy();

            // Play Scene������ ���Ǵ�  Model
            Container.Bind<PurchaseTowerModel>().AsCached().NonLazy();
            Container.Bind<WaveStatusModel>().AsCached().NonLazy(); // Wave ����
            Container.Bind<GoldModel>().AsCached().NonLazy();
            Container.Bind<ExpModel>().AsCached().NonLazy();
            Container.Bind<HpModel>().AsCached().NonLazy();
           
        }
    }
}
