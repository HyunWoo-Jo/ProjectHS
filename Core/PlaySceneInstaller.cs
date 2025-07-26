﻿using UnityEngine;
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
        [SerializeField] private GameObject _systemBase; // System을 가지고 있는 클레스

        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<GameDataHub>().AsCached().NonLazy();
            Container.Bind<GameObjectPoolManager>().FromNewComponentOn(this.gameObject).AsSingle().NonLazy();


            // Settings
            Container.Bind<StageSettingsModel>().AsCached();


            // Play Scene에서만 사용되는  Model
            Container.Bind<TowerPurchaseModel>().AsCached();
            Container.Bind<WaveStatusModel>().AsCached(); // Wave 정보
            Container.Bind<GoldModel>().AsCached();
            Container.Bind<ExpModel>().AsCached();
            Container.Bind<HpModel>().AsCached();
            Container.Bind<SelectedUpgradeModel>().AsCached();
            Container.Bind<TowerSaleModel>().AsCached();

            // System
            Container.BindInterfacesAndSelfTo<ScreenClickInputSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<CameraSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<MapSystem>().FromComponentOn(_systemBase).AsCached();

            Container.BindInterfacesAndSelfTo<StageSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<TowerSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<EnemySystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<WaveSystem>().FromComponentOn(_systemBase).AsCached();
            Container.BindInterfacesAndSelfTo<UpgradeSystem>().FromComponentOn(_systemBase).AsCached();

            
            // Service
            Container.Bind<ITowerPurchaseService>().To<TowerPurchaseService>().AsCached();
            Container.Bind<IRewardService>().To<RewardService>().AsCached();
            Container.Bind<ISellTowerService>().To<SellTowerService>().AsCached();
            Container.Bind<IUpgradeService>().To<UpgradeService>().AsCached();

            // Play Scene에서만 사용되는  View Model
            Container.BindInterfacesAndSelfTo<TowerPurchaseViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<WaveStatusViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<GoldViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<ExpViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<HpViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<PausePanelViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<UpgradeViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<RewardViewModel>().AsCached();
            Container.BindInterfacesAndSelfTo<SellTowerViewModel>().AsCached();


            // So 의존 주입
            Resources.LoadAll<UpgradeStrategyBaseSO>("UpgradeData").ToList().ForEach(Container.QueueForInject);
            Resources.LoadAll<UnlockStrategyBaseSO>("UpgradeData").ToList().ForEach(Container.QueueForInject);

        }
    }
}
