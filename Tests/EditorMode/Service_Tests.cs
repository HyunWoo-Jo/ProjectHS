using Data;
using GamePlay;
using NSubstitute;
using NUnit.Framework;
using System.Reflection;
using UI;
using UnityEngine;

namespace Tests.Service
{
    [TestFixture]
    public class GlobalUpgradePurchaseServiceTests {
        private GlobalUpgradePurchaseService _svc;
        private IGlobalUpgradeRepository _repo;
        private ICrystalRepository _cr;

        /// <summary>
        /// Substitute 초기화 및 서비스에 수동 DI.
        /// </summary>
        [SetUp]
        public void Setup() {
            _svc = new GlobalUpgradePurchaseService();
            _repo = Substitute.For<IGlobalUpgradeRepository>();
            _cr = Substitute.For<ICrystalRepository>();

            typeof(GlobalUpgradePurchaseService)
                .GetField("_globalUpgradeRepo", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _repo);

            typeof(GlobalUpgradePurchaseService)
                .GetField("_crystalRepo", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _cr);
        }

        /// <summary>
        /// 충분한 재화가 있을 때 구매가 성공하고
        /// SetLevel(level+1)이 호출되는지 검증한다.
        /// </summary>
        [Test]
        public void TryPurchase_Succeed() {
            const GlobalUpgradeType type = GlobalUpgradeType.Power;
            _repo.GetPrice(type).Returns(100);
            _repo.GetLevelLocal(type).Returns(2);
            _cr.GetValue().Returns(200);
            _cr.TrySpend(100).Returns(true);

            Assert.IsTrue(_svc.TryPurchase(type));
            _repo.Received(1).SetLevel(type, 3);
        }

        /// <summary>
        /// 재화 부족(또는 TrySpend 실패) 시 false를 반환하고
        /// SetLevel이 호출되지 않아야 한다.
        /// </summary>
        [Test]
        public void TryPurchase_Fail() {
            const GlobalUpgradeType type = GlobalUpgradeType.Power;
            _repo.GetPrice(type).Returns(300);
            _cr.GetValue().Returns(200);

            Assert.IsFalse(_svc.TryPurchase(type));
            _repo.DidNotReceive().SetLevel(Arg.Any<GlobalUpgradeType>(), Arg.Any<int>());
        }
    }

    [TestFixture]
    public class RewardServiceTests {
        private RewardService _svc;
        private IRewardPolicy _policy;
        private ICrystalRepository _cr;

        /// <summary>
        /// Substitute 초기화 및 서비스 주입.
        /// </summary>
        [SetUp]
        public void Setup() {
            _svc = new RewardService();
            _policy = Substitute.For<IRewardPolicy>();
            _cr = Substitute.For<ICrystalRepository>();

            typeof(RewardService)
                .GetField("_rewardPolicy", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _policy);
            typeof(RewardService)
                .GetField("_crystalRepo", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _cr);
        }

        /// <summary>
        /// ProcessFinalRewards가 처음 한 번만 TryEarn을 호출하고
        /// 두 번째 호출은 무시되는지 검증
        /// </summary>
        [Test]
        public void ProcessFinalRewards_Only_Once() {
            _policy.CalculateCrystalReward().Returns(50);

            _svc.ProcessFinalRewards();
            _svc.ProcessFinalRewards();

            _cr.Received(1).TryEarn(50);
        }

        /// <summary>
        /// 보상 값이 0 이하일 때 TryEarn을 호출하지 않아야 함
        /// </summary>
        [Test]
        public void ProcessFinalRewards_Skip_When_NonReward() {
            _policy.CalculateCrystalReward().Returns(0);
            _svc.ProcessFinalRewards();

            _cr.DidNotReceive().TryEarn(Arg.Any<int>());
        }
    }

    [TestFixture]
    public class SceneTransitionServiceTests {
        private SceneTransitionService _svc;
        private IUIFactory _factory;
        private ILoadManager _loader;
        private IWipeUI _wipeUI;

        /// <summary>
        /// Substitute 초기화 및 서비스 주입
        /// </summary>
        [SetUp]
        public void Setup() {
            _svc = new SceneTransitionService();
            _factory = Substitute.For<IUIFactory>();
            _loader = Substitute.For<ILoadManager>();
            _wipeUI = Substitute.For<IWipeUI>();

            // InstanceUI<WipeUI>() 호출 시 더미 반환
            _factory.InstanceUI<IWipeUI>(Arg.Any<int>()).Returns(_wipeUI);


            typeof(SceneTransitionService)
                .GetField("_uiFactory", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _factory);
            typeof(SceneTransitionService)
                .GetField("_loadManager", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _loader);
        }

        /// <summary>
        /// LoadScene 호출 시  
        /// Wipe UI 생성 및 효과 실행  
        /// LoadManager.LoadScene(scene, delay) 호출  
        /// 두 동작이 수행되는지 검증.
        /// </summary>
        [Test]
        public void LoadScene_Call_Wipe_And_LoadManager() {
            _svc.LoadScene(SceneName.PlayScene);

            _factory.Received(1).InstanceUI<IWipeUI>(100);
            _loader.Received(1).LoadScene(SceneName.PlayScene, 0.5f);
        }
    }

    [TestFixture]
    public class TowerPurchaseServiceTests {
        private TowerPurchaseService _svc;
        private GoldModel _gold;
        private ITowerPricePolicy _policy;
        private ITowerSystem _system;

        /// <summary>
        /// Substitute 초기화 및 서비스 주입.
        /// </summary>
        [SetUp]
        public void Setup() {
            _svc = new TowerPurchaseService();
            _gold = new GoldModel();
            _policy = Substitute.For<ITowerPricePolicy>();
            _system = Substitute.For<ITowerSystem>();

            typeof(TowerPurchaseService)
                .GetField("_gold", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _gold);
            typeof(TowerPurchaseService)
                .GetField("_pricePolicy", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _policy);
            typeof(TowerPurchaseService)
                .GetField("_towerSystem", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_svc, _system);
        }

        /// <summary>
        /// 골드가 충분하고 TowerSystem이 성공을 반환할 경우  
        /// </summary>
        [Test]
        public void TryPurchase_Succeed() {
            _gold.goldObservable.Value = 500;
            _policy.GetCurrentPrice().Returns(100);
            _system.TryAddTower().Returns(true);

            Assert.IsTrue(_svc.TryPurchase());
            Assert.AreEqual(400, _gold.goldObservable.Value);
            _policy.Received(1).AdvancePrice();
        }

        /// <summary>
        /// 골드 부족 시 구매 실패
        /// </summary>
        [Test]
        public void TryPurchase_EnoughGold() {
            _gold.goldObservable.Value = 50;
            _policy.GetCurrentPrice().Returns(100);

            Assert.IsFalse(_svc.TryPurchase());
            Assert.AreEqual(50, _gold.goldObservable.Value);
            _policy.DidNotReceive().AdvancePrice();
        }

        /// <summary>
        /// TowerSystem이 타워 추가 실패
        /// </summary>
        [Test]
        public void TryPurchase_Fail() {
            _gold.goldObservable.Value = 500;
            _policy.GetCurrentPrice().Returns(100);
            _system.TryAddTower().Returns(false);

            Assert.IsFalse(_svc.TryPurchase());
            Assert.AreEqual(500, _gold.goldObservable.Value);
            _policy.DidNotReceive().AdvancePrice();
        }
    }
}
