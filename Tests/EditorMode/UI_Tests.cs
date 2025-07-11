using System;
using System.Collections;
using System.Configuration;
using System.Reflection;
using GamePlay;
using NUnit.Framework;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using Zenject;
using Contracts;
using NSubstitute;
using Data;

namespace Tests.UI
{
    [TestFixture]
    public class MainLobbyNavigateViewModelTests {
        private MainLobbyNavigateViewModel _vm;
        private ISceneTransitionService _sts;

        /// <summary>
        /// ViewModel 인스턴스 및 Substitute 초기화, 수동 DI 수행
        /// </summary>
        [SetUp]
        public void Setup() {
            _vm = new MainLobbyNavigateViewModel();
            _sts = Substitute.For<ISceneTransitionService>();

            // 수동 주입
            typeof(MainLobbyNavigateViewModel)
                .GetField("_sts", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _sts);
        }

        /// <summary>
        /// 패널 상태(Current·Pre) 업데이트, OnDataChanged 이벤트 발생 검증
        /// </summary>
        [Test]
        public void OnClickPanelMoveButton_Event() {
            var raised = false;
            _vm.OnDataChanged += () => raised = true;

            const MainLobbyNavigateViewModel.PanelType target =
                MainLobbyNavigateViewModel.PanelType.Upgrade;

            _vm.OnClickPanelMoveButton(target);

            Assert.IsTrue(raised, "OnDataChanged 이벤트가 발생해야 합니다.");
            Assert.AreEqual(target, _vm.CurrentActivePanel);
            Assert.AreEqual(target, _vm.PreActivePanel);
        }

        /// <summary>
        /// ChangeScene이 Service.LoadScene(PlayScene)을 호출하는지 검증
        /// </summary>
        [Test]
        public void ChangeScene_LoadScene() {
            _vm.ChangeScene();
            _sts.Received(1).LoadScene(SceneName.PlayScene);
        }

    }

    [TestFixture]
    public class RewardViewModelTests {
        private RewardViewModel _vm;
        private IRewardService _rewardService;
        private ISceneTransitionService _sts;      

        [SetUp]
        public void Setup() {
            _vm = new RewardViewModel();
            _rewardService = Substitute.For<IRewardService>();
            _sts = Substitute.For<ISceneTransitionService>();
            

            // 수동 주입
            typeof(RewardViewModel)!
            .GetField("_rewardService", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _rewardService);

            typeof(RewardViewModel)!
                .GetField("_sts", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _sts);
        }
        /// <summary>
        /// 최종 보상 처리 테스트 이벤트 발생 및 서비스 호출 여부 확인
        /// </summary>
        [Test]
        public void ProcessFinalReward_Service() {
            _rewardService.CalculateRewardCrystal().Returns(77);
            int raised = -1;
            _vm.OnDataChanged += v => raised = v;

            _vm.ProcessFinalReward();

            Assert.AreEqual(77, raised, "이벤트 값을 잘못 전달했습니다.");

            _rewardService.Received(1).ProcessFinalRewards();
        }

        /// <summary>
        /// ChangeScene이 Service.LoadScene(MainLobbyScene)을 호출하는지 검증
        /// </summary>
        [Test]
        public void ChangeScene_LoadScene() {
            _vm.ChangeScene();
            _sts.Received(1).LoadScene(SceneName.MainLobbyScene);
        }
    }
    [TestFixture]
    public class PausePanelViewModelTests {
        private PausePanelViewModel _vm;
        private ISceneTransitionService _sts;
        private WaveStatusModel _wsModel;
        [SetUp]
        public void Setup() {
            _vm = new PausePanelViewModel();
            _sts = Substitute.For<ISceneTransitionService>();
            _wsModel = new WaveStatusModel();

            // 수동 주입
            typeof(PausePanelViewModel)!
            .GetField("_sts", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _sts);

            typeof(PausePanelViewModel)!
                .GetField("_waveStatusModel", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _wsModel);
        }


        /// <summary>
        /// ChangeScene이 Service.LoadScene(MainLobbyScene)을 호출하는지 검증
        /// </summary>
        [Test]
        public void ChangeScene_LoadScene() {
            _vm.ChangeScene();
            _sts.Received(1).LoadScene(SceneName.MainLobbyScene);
        }

    }

    [TestFixture]
    public class MainLobbyUpgradeViewModelTests {
        private MainLobbyUpgradeViewModel _vm;
        private IGlobalUpgradePurchaseService _ps;
        private IGlobalUpgradeRepository _repo;
        private Action _capturedListener;
        [SetUp]
        public void Setup() {
            _vm = new MainLobbyUpgradeViewModel();
            _ps = Substitute.For<IGlobalUpgradePurchaseService>();
            _repo = Substitute.For<IGlobalUpgradeRepository>();

            // 수동 주입
            typeof(MainLobbyUpgradeViewModel)!
            .GetField("_purchaseService", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _ps);

            typeof(MainLobbyUpgradeViewModel)!
            .GetField("_repo", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _repo);

            _repo.When(r => r.AddChangedListener(Arg.Any<Action>()))
                .Do(ci => _capturedListener = ci.Arg<Action>());
        }


        /// <summary>
        /// TryPurchase가 Service 위임·리턴 검증
        /// </summary>
        [Test]
        public void TryPurchase_Service() {
            var type = GlobalUpgradeType.Power;
            _ps.TryPurchase(type).Returns(true);

            var result = _vm.TryPurchase(type);

            Assert.IsTrue(result);
            _ps.Received(1).TryPurchase(type);
        }

        /// <summary>
        /// repo에 이벤트가 등록 되는지 확인
        /// </summary>
        [Test]
        public void Initialize_Registers_RepoListener() {
            _vm.Initialize();

            _repo.Received(1).AddChangedListener(Arg.Any<Action>());
        }
        /// <summary>
        /// repo에 이벤트가 해제되는지 확인
        /// </summary>
        [Test]
        public void Dispose_Removes_RepoListener() {
            _vm.Initialize();
            _vm.Dispose();

            // 델리게이트가 전달됐는지 확인
            _repo.Received(1).RemoveChangedListener(_capturedListener);
        }

        /// <summary>
        /// Repo가 이벤트가 전달되는지 확인
        /// </summary>
        [Test]
        public void Notification_OnDataChanged() {
            _vm.Initialize();

            var raised = false;
            _vm.OnDataChanged += () => raised = true;

            // 리스너 직접 호출
            _capturedListener.Invoke();

            Assert.IsTrue(raised);
        }
    }

    [TestFixture]
    public class UpgradeViewModelTests {
        private UpgradeViewModel _vm;
        private SelectedUpgradeModel _model;
        private IUpgradeService _us;

        [SetUp]
        public void Setup() {
            _vm = new UpgradeViewModel();
            _model = new SelectedUpgradeModel();
            _us = Substitute.For<IUpgradeService>();

            // 수동 주입
            typeof(UpgradeViewModel)!
            .GetField("_model", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _model);

            typeof(UpgradeViewModel)!
            .GetField("_upgradeService", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _us);

        }
        /// <summary>
        /// SelectUpgrade가 ApplyUpgrade로 위임되는지 검증
        /// </summary>
        [Test]
        public void SelectUpgrade_Service() {
            const int idx = 0;
            _vm.SelectUpgrade(idx);
            _us.Received(1).ApplyUpgrade(idx);
        }

        /// <summary>
        /// Initialize 이후 Model 값 변경이 OnDataChanged 이벤트로 전파되는지 검증
        /// </summary>
        [Test]
        public void Initialize_DataChange() {
            _vm.Initialize();

            var raised = false;
            _vm.OnDataChanged += _ => raised = true;

            // 값 할당 내부에서 이벤트 발생
            _model.observableUpgradeDatas[0].Value = ScriptableObject.CreateInstance<UpgradeDataSO>();

            Assert.IsTrue(raised);
        }

        /// <summary>
        /// Dispose 호출 시 repo에 이벤트가 해제되는지 검증
        /// </summary>
        [Test]
        public void Dispose_Listeners() {
            _vm.Initialize();
            _vm.Dispose();

            var raised = false;
            _vm.OnDataChanged += _ => raised = true;

            // Dispose 이후 값 변경 이벤트가 더 이상 전달되지 않아야 함
            _model.observableUpgradeDatas[0].Value = ScriptableObject.CreateInstance<UpgradeDataSO>();
            Assert.IsFalse(raised);
        }

    }

    [TestFixture]
    public class TowerPurchaseViewModelTests {
        private TowerPurchaseViewModel _vm;
        private TowerPurchaseModel _model;
        private ITowerPurchaseService _svc;

        [SetUp]
        public void Setup() {
            _vm = new TowerPurchaseViewModel();
            _model = new TowerPurchaseModel();           
            _svc = Substitute.For<ITowerPurchaseService>();

            // 수동 주입
            typeof(TowerPurchaseViewModel)
                .GetField("_model", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _model);

            typeof(TowerPurchaseViewModel)
                .GetField("_towerPurchaseService", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _svc);

        }

        /// <summary>
        /// ITowerPurchaseService.TryPurchase 호출 검증
        /// </summary>
        [Test]
        public void PurchaseButtonClick_Service() {
            _vm.Initialize();
            _svc.TryPurchase().Returns(true);

            var result = _vm.PurchaseButtonClick();

            Assert.IsTrue(result);
            _svc.Received(1).TryPurchase();
        }

        /// <summary>
        /// Model 가격이 변경되면 OnDataChanged 이벤트가 전파되는지 검증.
        /// </summary>
        [Test]
        public void Initialize_Price_Change() {
            _vm.Initialize();

            var raised = false;
            var newPrice = 999;
            _vm.OnDataChanged += p => { raised = true; Assert.AreEqual(newPrice, p); };

            _model.towerPriceObservable.Value = newPrice;   // setter → 이벤트 내부 Raise

            Assert.IsTrue(raised);
        }

        /// <summary>
        /// Dispose 호출 뒤에는 이벤트가 해제되어 더 이상 전달되지 않는지 검증.
        /// </summary>
        [Test]
        public void Dispose_Listeners() {
            _vm.Initialize();
            _vm.Dispose();

            var raised = false;
            _vm.OnDataChanged += _ => raised = true;

            _model.towerPriceObservable.Value = 123;   // 이벤트 전달 X

            Assert.IsFalse(raised);
        }
    }
}
