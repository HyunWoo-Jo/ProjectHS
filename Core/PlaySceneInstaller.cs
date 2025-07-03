using UnityEngine;
using Zenject;
using Data;
using GamePlay;
using UI;
using Contracts;
using System.Linq;
namespace Core
{
    public class PlaySceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _systemBase; // System�� ������ �ִ� Ŭ����

        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<GameDataHub>().AsCached().NonLazy();
            Container.Bind<GameObjectPoolManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();


            // Settings
            Container.Bind<StageSettingsModel>().AsCached();


            // Play Scene������ ���Ǵ�  Model
            Container.Bind<TowerPurchaseModel>().AsCached();
            Container.Bind<WaveStatusModel>().AsCached(); // Wave ����
            Container.Bind<GoldModel>().AsCached();
            Container.Bind<ExpModel>().AsCached();
            Container.Bind<HpModel>().AsCached();
            Container.Bind<SelectedUpgradeModel>().AsCached();

            // System
            Container.BindInterfacesAndSelfTo<ScreenClickInputSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<CameraSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<MapSystem>().FromComponentOn(_systemBase).AsCached();

            Container.BindInterfacesAndSelfTo<StageSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<TowerSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<EnemySystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<WaveSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<UpgradeSystem>().FromComponentOn(_systemBase).AsCached();

            // Policy
            Container.Bind<IGoldPolicy>().To<GoldPolicy>().AsCached();
            Container.Bind<IHpPolicy>().To<HpPolicy>().AsCached();
            Container.Bind<IExpPolicy>().To<ExpPolicy>().AsCached();
            Container.Bind<ITowerPricePolicy>().To<TowerPricePolicy>().AsCached();
            // Service
            Container.Bind<ITowerPurchaseService>().To<TowerPurchaseService>().AsCached();


            // Play Scene������ ���Ǵ�  View Model
            Container.BindInterfacesAndSelfTo<TowerPurchaseViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<WaveStatusViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<GoldViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<ExpViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<HpViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<PausePanelViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<UpgradeViewModel>().AsSingle();


            // So ���� ����
            Resources.LoadAll<UpgradeStrategyBaseSO>("UpgradeData").ToList().ForEach(Container.QueueForInject);
            Resources.LoadAll<UnlockStrategyBaseSO>("UpgradeData").ToList().ForEach(Container.QueueForInject);

        }
    }
}
