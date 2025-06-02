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
            Container.Bind<PurchaseTowerViewModel>().AsTransient().NonLazy();
            Container.BindInterfacesAndSelfTo<WaveStatusViewModel>().AsCached().NonLazy();
            // Play Scene������ ���Ǵ�  Model
            Container.Bind<WaveStatusModel>().AsCached().NonLazy(); // Wave ����
        }
    }
}
