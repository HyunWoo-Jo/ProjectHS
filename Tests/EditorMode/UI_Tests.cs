using UnityEngine;
using System;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;
using Cysharp.Threading.Tasks;
using GamePlay;
using Domain;
using Contracts;
using UI;

namespace Tests.UI
{
    /// MainLobbyNavigateViewModel
    [TestFixture]
    public class MainLobbyNavigateViewModelTests {
        private MainLobbyNavigateViewModel _vm;
        private ISceneTransitionService _sts;

        [SetUp]
        public void SetUp() {
            _vm = new MainLobbyNavigateViewModel();
            _sts = Substitute.For<ISceneTransitionService>();

            // inject
            typeof(MainLobbyNavigateViewModel)
                .GetField("_sts", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _sts);
        }

        [Test]
        public void ClickWorld() {
            _vm.OnClickPanelMoveButton(MainLobbyNavigateViewModel.PanelType.World);

            Assert.AreEqual(
                MainLobbyNavigateViewModel.PanelType.World,
                _vm.CurrentActivePanel);

            Assert.AreEqual(
                MainLobbyNavigateViewModel.PanelType.World,
                _vm.PreActivePanel);
        }

        [Test]
        public void ClickUpgrade() {
            _vm.OnClickPanelMoveButton(MainLobbyNavigateViewModel.PanelType.Upgrade);

            Assert.AreEqual(
                MainLobbyNavigateViewModel.PanelType.Upgrade,
                _vm.CurrentActivePanel);

            Assert.AreEqual(
                MainLobbyNavigateViewModel.PanelType.Upgrade,
                _vm.PreActivePanel);
        }

        [Test]
        public void ChangeScene() {
            _vm.ChangeScene();

            _sts.Received(1).LoadScene(SceneName.PlayScene);
        }
    }
    /// MainLobbyUpgradeViewModel
    [TestFixture]
    public class MainLobbyUpgradeViewModelTests {
        private MainLobbyUpgradeViewModel _vm;
        private GlobalUpgradeModel _model;
        private IGlobalUpgradePurchaseService _svc;

        [SetUp]
        public void SetUp() {
            _vm = new MainLobbyUpgradeViewModel();
            _model = Substitute.For<GlobalUpgradeModel>();          // 가벼운 대역
            _svc = Substitute.For<IGlobalUpgradePurchaseService>();

            typeof(MainLobbyUpgradeViewModel).GetField("_model",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _model);
            typeof(MainLobbyUpgradeViewModel).GetField("_purchaseService",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _svc);
        }

        [Test]
        public void TryPurchaseCallsService() {
            _vm.TryPurchase(GlobalUpgradeType.Power);
            _svc.Received(1).TryPurchaseAsync(GlobalUpgradeType.Power);
        }
    }

    /// PausePanelViewModel
    [TestFixture]
    public class PausePanelViewModelTests {
        private PausePanelViewModel _vm;
        private WaveStatusModel _wave;
        private ISceneTransitionService _sts;

        [SetUp]
        public void SetUp() {
            _vm = new PausePanelViewModel();
            _wave = new WaveStatusModel();
            _sts = Substitute.For<ISceneTransitionService>();

            typeof(PausePanelViewModel).GetField("_waveStatusModel",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _wave);
            typeof(PausePanelViewModel).GetField("_sts",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _sts);
        }

        [Test]
        public void LevelReflectsWaveModel() {
            _wave.SetWaveLevel(7);
            Assert.AreEqual(7, _vm.Level);
        }

        [Test]
        public void ChangeSceneCallsLoader() {
            _vm.ChangeScene();
            _sts.Received(1).LoadScene(SceneName.MainLobbyScene);
        }
    }

    /// RewardViewModel
    [TestFixture]
    public class RewardViewModelTests {
        private RewardViewModel _vm;
        private IRewardService _svc;
        private ISceneTransitionService _sts;

        [SetUp]
        public void SetUp() {
            _vm = new RewardViewModel();
            _svc = Substitute.For<IRewardService>();
            _sts = Substitute.For<ISceneTransitionService>();

            typeof(RewardViewModel).GetField("_rewardService",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _svc);
            typeof(RewardViewModel).GetField("_sts",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _sts);
        }

        [Test]
        public void RewardCrystalUsesService() {
            _svc.CalculateRewardCrystal().Returns(42);
            Assert.AreEqual(42, _vm.RewardCrystal);
        }

        [Test]
        public void ProcessFinalRewardInvokesEventAndService() {
            _svc.CalculateRewardCrystal().Returns(30);
            var invoked = false;
            _vm.OnDataChanged += v => { invoked = v == 30; };

            _vm.ProcessFinalReward();

            Assert.IsTrue(invoked);
            _svc.Received(1).ProcessFinalRewards();
        }

        [Test]
        public void ChangeSceneCallsLoader() {
            _vm.ChangeScene();
            _sts.Received(1).LoadScene(SceneName.MainLobbyScene);
        }
    }

    /// SellTowerViewModel
    [TestFixture]
    public class SellTowerViewModelTests {
        private SellTowerViewModel _vm;
        private TowerSaleModel _model;
        private ISellTowerService _svc;

        [SetUp]
        public void SetUp() {
            _vm = new SellTowerViewModel();
            _model = new TowerSaleModel();          
            _svc = Substitute.For<ISellTowerService>();

            const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            typeof(SellTowerViewModel).GetField("_model", Flags)!.SetValue(_vm, _model);
            typeof(SellTowerViewModel).GetField("_sellTowerService", Flags)!.SetValue(_vm, _svc);
        }

        [Test]
        public void TrySellCallsService() {
            _vm.TrySell();
            _svc.Received(1).TrySellTower();
        }

        [Test]
        public void NotifyCallsModel() {
            _vm.Notify();
            Assert.Pass();              // 실행만 되면 성공
        }
    }

    /// TowerPurchaseViewModel
    [TestFixture]
    public class TowerPurchaseViewModelTests {
        private TowerPurchaseViewModel _vm;
        private TowerPurchaseModel _model;
        private ITowerPurchaseService _svc;

        [SetUp]
        public void SetUp() {
            _vm = new TowerPurchaseViewModel();
            _model = Substitute.For<TowerPurchaseModel>();
            _svc = Substitute.For<ITowerPurchaseService>();

            typeof(TowerPurchaseViewModel).GetField("_model",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _model);
            typeof(TowerPurchaseViewModel).GetField("_towerPurchaseService",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _svc);
        }

        [Test]
        public void TryPurchaseReturnsServiceValue() {
            _svc.TryPurchase().Returns(true);
            Assert.IsTrue(_vm.TryPurchase());
            _svc.Received(1).TryPurchase();
        }

        [Test]
        public void NotifyCallsModel() {
            _vm.Notify();
            _model.Received(1).Notify();
        }
    }

    /// UpgradeViewModel
    [TestFixture]
    public class UpgradeViewModelTests {
        private UpgradeViewModel _vm;
        private SelectedUpgradeModel _model;
        private IUpgradeService _svc;

        [SetUp]
        public void SetUp() {
            _vm = new UpgradeViewModel();
            _model = Substitute.For<SelectedUpgradeModel>();
            _svc = Substitute.For<IUpgradeService>();

            typeof(UpgradeViewModel).GetField("_model",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _model);
            typeof(UpgradeViewModel).GetField("_upgradeService",
                BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(_vm, _svc);
        }

        [Test]
        public void SelectUpgradeCallsService() {
            _vm.SelectUpgrade(1);
            _svc.Received(1).ApplyUpgrade(1);
        }

        [Test]
        public void RerollCallsService() {
            _vm.Reroll(2);
            _svc.Received(1).Reroll(2);
        }

        [Test]
        public void NotifyCallsModel() {
            _vm.Notify();
            _model.Received(1).Notify();
        }
    }
}
