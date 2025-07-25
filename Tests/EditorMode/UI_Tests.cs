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
using R3;

namespace Tests.UI
{
    [TestFixture]
    public class MainLobbyNavigateViewModelTests {
        private MainLobbyNavigateViewModel _vm;
        private ISceneTransitionService _sts;

        [SetUp]
        public void Setup() {
            _vm = new MainLobbyNavigateViewModel();
            _sts = Substitute.For<ISceneTransitionService>();

            // ���� DI
            typeof(MainLobbyNavigateViewModel)
                .GetField("_sts", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _sts);
        }

        /// <summary>
        /// ��ư Ŭ�� �� ReactiveProperty �� & PreActivePanel �� �ùٸ��� ���ŵǴ��� ����
        /// </summary>
        [Test]
        public void OnClick_UpdatesState() {
            // ��ȭ ������ ���� ����
            MainLobbyNavigateViewModel.PanelType observed = _vm.CurrentActivePanel;
            var d = _vm.RO_CurrentActivePanelObservable
                       .Skip(1)                    // �ʱ� �� ��ŵ
                       .Subscribe(x => observed = x);

            const MainLobbyNavigateViewModel.PanelType target =
                MainLobbyNavigateViewModel.PanelType.Upgrade;

            // �޼��� ȣ��
            _vm.OnClickPanelMoveButton(target);

            // ����
            Assert.AreEqual(target, observed, "ReactiveProperty ���� ���ŵž� �մϴ�.");
            Assert.AreEqual(target, _vm.CurrentActivePanel);
            Assert.AreEqual(target, _vm.PreActivePanel);

            d.Dispose(); // ���� ����
        }

        /// <summary>
        /// Service�� 1ȸ ȣ���ϴ��� ����
        /// </summary>
        [Test]
        public void ChangeScene_CallsLoadScene() {
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
            

            // ���� ����
            typeof(RewardViewModel)!
            .GetField("_rewardService", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _rewardService);

            typeof(RewardViewModel)!
                .GetField("_sts", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _sts);
        }
        /// <summary>
        /// ���� ���� ó�� �׽�Ʈ �̺�Ʈ �߻� �� ���� ȣ�� ���� Ȯ��
        /// </summary>
        [Test]
        public void ProcessFinalReward_Service() {
            _rewardService.CalculateRewardCrystal().Returns(77);
            int raised = -1;
            _vm.OnDataChanged += v => raised = v;

            _vm.ProcessFinalReward();

            Assert.AreEqual(77, raised, "�̺�Ʈ ���� �߸� �����߽��ϴ�.");

            _rewardService.Received(1).ProcessFinalRewards();
        }

        /// <summary>
        /// ChangeScene�� Service.LoadScene(MainLobbyScene)�� ȣ���ϴ��� ����
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

            // ���� ����
            typeof(PausePanelViewModel)!
            .GetField("_sts", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _sts);

            typeof(PausePanelViewModel)!
                .GetField("_waveStatusModel", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _wsModel);
        }


        /// <summary>
        /// ChangeScene�� Service.LoadScene(MainLobbyScene)�� ȣ���ϴ��� ����
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

            // ���� ����
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
        /// TryPurchase�� Service ���ӡ����� ����
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
        /// Repo�� �̺�Ʈ�� ���޵Ǵ��� Ȯ��
        /// </summary>
        [Test]
        public void Notification_OnDataChanged() {
            _vm.Initialize();

            var raised = false;
            _vm.OnDataChanged += () => raised = true;

            // ������ ���� ȣ��
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

            // ���� ����
            typeof(UpgradeViewModel)!
            .GetField("_model", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _model);

            typeof(UpgradeViewModel)!
            .GetField("_upgradeService", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(_vm, _us);

        }
        /// <summary>
        /// SelectUpgrade�� ApplyUpgrade�� ���ӵǴ��� ����
        /// </summary>
        [Test]
        public void SelectUpgrade_Service() {
            const int idx = 0;
            _vm.SelectUpgrade(idx);
            _us.Received(1).ApplyUpgrade(idx);
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

            // ���� ����
            typeof(TowerPurchaseViewModel)
                .GetField("_model", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _model);

            typeof(TowerPurchaseViewModel)
                .GetField("_towerPurchaseService", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(_vm, _svc);

        }

        /// <summary>
        /// ITowerPurchaseService.TryPurchase ȣ�� ����
        /// </summary>
        [Test]
        public void PurchaseButtonClick_Service() {
            _svc.TryPurchase().Returns(true);

            var result = _vm.TryPurchase();

            Assert.IsTrue(result);
            _svc.Received(1).TryPurchase();
        }


    }
}
