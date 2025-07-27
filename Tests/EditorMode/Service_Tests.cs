using Contracts;
using Data;
using GamePlay;
using NSubstitute;
using NUnit.Framework;
using UI;
using UnityEngine;
using Domain;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
namespace Tests.Service
{
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using NSubstitute;
    using NUnit.Framework;
    using Domain;
    using Contracts;
    using System.Collections.Generic;
    using System.Reflection;

    // GlobalUpgradePurchaseService
    [TestFixture]
    public class GlobalUpgradePurchaseServiceTests {

        private GlobalUpgradePurchaseService _svc;
        private GlobalUpgradeModel _upgrade;
        private CrystalModel _crystal;

        private IGlobalUpgradeRepository _upRepo;
        private ICrystalRepository _crRepo;

        [SetUp]
        public void Setup() {
            _svc = new GlobalUpgradePurchaseService();
            _upgrade = new GlobalUpgradeModel();
            _crystal = new CrystalModel();

            _upRepo = Substitute.For<IGlobalUpgradeRepository>();
            _crRepo = Substitute.For<ICrystalRepository>();

            // inject repo into models
            typeof(GlobalUpgradeModel)
                .GetField("_repo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_upgrade, _upRepo);

            typeof(CrystalModel)
                .GetField("_repo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_crystal, _crRepo);

            // inject models into service
            typeof(GlobalUpgradePurchaseService)
                .GetField("_globalUpgradModel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_svc, _upgrade);

            typeof(GlobalUpgradePurchaseService)
                .GetField("_crystalModel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_svc, _crystal);
        }

        [Test]
        public async Task PurchaseSuccess() {
            const GlobalUpgradeType key = GlobalUpgradeType.Power; // 실제 enum 값 사용

            // repo stubs
            _upRepo.LoadTableAsync().Returns(UniTask.CompletedTask);
            _upRepo.LoadAllUpgradeLevelAsync()
                  .Returns(UniTask.FromResult(new Dictionary<string, int> { { key.ToString(), 1 } }));

            _upRepo.GetLevelAsync(key).Returns(UniTask.FromResult(1));
            _upRepo.GetPrice(key, 1).Returns(100);

            _crRepo.GetAsyncValue().Returns(UniTask.FromResult(200));
            _crRepo.AsyncTrySpend(100).Returns(UniTask.FromResult(true));

            // prime dictionary
            await _upgrade.AsyncLoadData();

            var ok = await _svc.TryPurchaseAsync(key);

            Assert.IsTrue(ok);
            _upRepo.Received(1).SetLevel(key, 2);
        }

        [Test]
        public async Task PurchaseFailCrystal() {
            const GlobalUpgradeType key = GlobalUpgradeType.Power;

            _upRepo.LoadTableAsync().Returns(UniTask.CompletedTask);
            _upRepo.LoadAllUpgradeLevelAsync()
                  .Returns(UniTask.FromResult(new Dictionary<string, int> { { key.ToString(), 1 } }));

            _upRepo.GetLevelAsync(key).Returns(UniTask.FromResult(1));
            _upRepo.GetPrice(key, 1).Returns(300);

            _crRepo.GetAsyncValue().Returns(UniTask.FromResult(200));

            await _upgrade.AsyncLoadData();

            var ok = await _svc.TryPurchaseAsync(key);

            Assert.IsFalse(ok);
            _upRepo.DidNotReceive().SetLevel(key, Arg.Any<int>());
        }
    }

    /// TowerPurchaseService
    [TestFixture]
    public class TowerPurchaseServiceTests {
        private TowerPurchaseService _svc;
        private ITowerSystem _towerSystem;

        private GoldModel _goldModel;
        private IGoldPolicy _goldPolicy;

        private TowerPurchaseModel _priceModel;
        private ITowerPricePolicy _pricePolicy;

        [SetUp]
        public void SetUp() {
            _svc = new TowerPurchaseService();
            _towerSystem = Substitute.For<ITowerSystem>();

            _goldModel = new GoldModel();
            _goldPolicy = Substitute.For<IGoldPolicy>();

            _priceModel = new TowerPurchaseModel();
            _pricePolicy = Substitute.For<ITowerPricePolicy>();

            // policy inject
            typeof(GoldModel)
                .GetField("_policy", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_goldModel, _goldPolicy);

            typeof(TowerPurchaseModel)
                .GetField("_towerPricePolicy", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_priceModel, _pricePolicy);

            // service inject
            typeof(TowerPurchaseService)
                .GetField("_goldModel", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _goldModel);

            typeof(TowerPurchaseService)
                .GetField("_towerPurchaseModel", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _priceModel);

            typeof(TowerPurchaseService)
                .GetField("_towerSystem", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _towerSystem);

            // initial data
            _goldModel.InitializeGold(100);
            _priceModel.SetTowerPrice(50);
        }

        [Test]
        public void PurchaseSuccess() {
            _towerSystem.SerchEmptySlot().Returns(2);

            _goldPolicy.TrySpendGold(100, 50, out Arg.Any<int>())
                       .Returns(x => { x[2] = 50; return true; });

            _pricePolicy.AdvancePrice(50).Returns(60);

            bool ok = _svc.TryPurchase();

            Assert.IsTrue(ok);
            _towerSystem.Received(1).AddTower(2);
            Assert.AreEqual(50, _goldModel.Gold);           // 100 - 50
            Assert.AreEqual(60, _priceModel.TowerPrice);    // advanced
        }

        [Test]
        public void NoSlot() {
            _towerSystem.SerchEmptySlot().Returns(-1);

            bool ok = _svc.TryPurchase();

            Assert.IsFalse(ok);
            _towerSystem.DidNotReceive().AddTower(Arg.Any<int>());
        }

        [Test]
        public void SpendFail() {
            _towerSystem.SerchEmptySlot().Returns(1);

            _goldPolicy.TrySpendGold(Arg.Any<int>(), 50, out Arg.Any<int>())
                       .Returns(x => { x[2] = 0; return false; });

            bool ok = _svc.TryPurchase();

            Assert.IsFalse(ok);
            _towerSystem.DidNotReceive().AddTower(Arg.Any<int>());
        }
    }

    /// SellTowerService
    [TestFixture]
    public class SellTowerServiceTests {
        private SellTowerService _svc;
        private ITowerSystem _towerSystem;
        private GoldModel _goldModel;
        private IGoldPolicy _goldPolicy;

        [SetUp]
        public void SetUp() {
            _svc = new SellTowerService();
            _towerSystem = Substitute.For<ITowerSystem>();
            _goldModel = new GoldModel();
            _goldPolicy = Substitute.For<IGoldPolicy>();

            // inject policy into gold model
            typeof(GoldModel)
                .GetField("_policy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_goldModel, _goldPolicy);

            // inject dependencies into service
            typeof(SellTowerService)
                .GetField("_towerSystem", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_svc, _towerSystem);

            typeof(SellTowerService)
                .GetField("_goldModel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_svc, _goldModel);
        }

        [Test]
        public void SellSuccess() {
            // arrange
            _towerSystem.TryRemoveTower(out Arg.Any<int>())
                        .Returns(x => { x[0] = 50; return true; });

            _goldPolicy.TryEarnGold(Arg.Any<int>(), 50, out Arg.Any<int>())
                       .Returns(x => { x[2] = 50; return true; });

            int before = _goldModel.Gold;

            // act
            bool ok = _svc.TrySellTower();

            // assert
            Assert.IsTrue(ok);
            Assert.AreEqual(before + 50, _goldModel.Gold);
            _goldPolicy.Received(1).TryEarnGold(before, 50, out Arg.Any<int>());
        }

        [Test]
        public void SellFail() {
            // arrange
            _towerSystem.TryRemoveTower(out Arg.Any<int>()).Returns(false);
            int before = _goldModel.Gold;

            // act
            bool ok = _svc.TrySellTower();

            // assert
            Assert.IsFalse(ok);
            Assert.AreEqual(before, _goldModel.Gold); // 골드 그대로
            _goldPolicy.DidNotReceive().TryEarnGold(Arg.Any<int>(), Arg.Any<int>(), out Arg.Any<int>());
        }
    }

    /// SceneTransitionService
    [TestFixture]
    public class SceneTransitionServiceTests {
        SceneTransitionService svc;
        IUIFactory ui;
        ILoadManager loader;

        [SetUp]
        public void Setup() {
            svc = new SceneTransitionService();
            ui = Substitute.For<IUIFactory>();
            loader = Substitute.For<ILoadManager>();

            typeof(SceneTransitionService).GetField("_uiFactory",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.SetValue(svc, ui);
            typeof(SceneTransitionService).GetField("_loadManager",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.SetValue(svc, loader);

            ui.InstanceUI<IWipeUI>(Arg.Any<int>()).Returns(Substitute.For<IWipeUI>());
        }

        [Test]
        public void LoadSceneCall() {
            svc.LoadScene(SceneName.PlayScene);
            ui.Received(1).InstanceUI<IWipeUI>(100);
            loader.Received(1).LoadScene(SceneName.PlayScene, Arg.Any<float>());
        }
    }

    /// RewardService
    [TestFixture]
    public class RewardServiceTests {
        private RewardService _svc;
        private IRewardPolicy _policy;
        private WaveStatusModel _wave;
        private CrystalModel _crystal;
        private ICrystalRepository _repo;

        [SetUp]
        public void Setup() {
            _svc = new RewardService();
            _policy = Substitute.For<IRewardPolicy>();
            _wave = new WaveStatusModel();
            _crystal = new CrystalModel();
            _repo = Substitute.For<ICrystalRepository>();

            // inject repo into crystal
            typeof(CrystalModel)
                .GetField("_repo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_crystal, _repo);

            // inject dependencies into service
            typeof(RewardService)
                .GetField("_rewardPolicy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_svc, _policy);

            typeof(RewardService)
                .GetField("_crystalModel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_svc, _crystal);

            typeof(RewardService)
                .GetField("_waveModel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(_svc, _wave);
        }

        [Test]
        public void Calc() {
            _wave.SetWaveLevel(3);
            _policy.CalculateCrystalReward(3).Returns(45);

            var value = _svc.CalculateRewardCrystal();

            Assert.AreEqual(45, value);
        }

        [Test]
        public void RewardPositive() {
            _wave.SetWaveLevel(4);
            _policy.CalculateCrystalReward(4).Returns(60);

            _repo.AsyncTryEarn(60).Returns(UniTask.FromResult(true));
            _repo.GetAsyncValue().Returns(UniTask.FromResult(0));

            _svc.ProcessFinalRewards();

            _repo.Received(1).AsyncTryEarn(60);
        }

        [Test]
        public void RewardZero() {
            _wave.SetWaveLevel(2);
            _policy.CalculateCrystalReward(2).Returns(0);

            _svc.ProcessFinalRewards();

            _repo.DidNotReceive().AsyncTryEarn(Arg.Any<int>());
        }
    }
}
