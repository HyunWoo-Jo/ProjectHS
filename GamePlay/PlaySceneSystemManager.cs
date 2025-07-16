using UnityEngine;
using Data;
using System.Linq;
using Zenject;
using System;
using UI;
using UnityEngine.Assertions;
using CustomUtility;
using Contracts;
namespace GamePlay
{
    /// <summary>
    /// System�� ����
    /// </summary>
    [DefaultExecutionOrder(100)] // System �� ���� ���Ŀ� �����ǵ��� ����
    [RequireComponent (typeof(MapSystem))]
    [RequireComponent (typeof(ScreenClickInputSystem))]
    [RequireComponent (typeof(CameraSystem))]
    [RequireComponent (typeof(StageSystem))]
    [RequireComponent (typeof(WaveSystem))]
    [RequireComponent (typeof(EnemySystem))]
    [RequireComponent (typeof(TowerSystem))]
    public class PlaySceneSystemManager : MonoBehaviour
    {
        [Inject] private GameDataHub _gameDataHub;

        private MapSystem _mapSystem;
        private ScreenClickInputSystem _inputSystem;
        private CameraSystem _cameraSystem;
        private StageSystem _stageSystem;
        private WaveSystem _waveSystem;
        private EnemySystem _enemySystem;
        private TowerSystem _towerSystem;
        private UpgradeSystem _upgradeSystem;

        public Vector2Int _mapSize = new Vector2Int(10, 10); // �ӽ� �� ������



        /// Model
        [Inject] private TowerPurchaseModel _towerPurchaseModel; // Ÿ�� ���� ��� ��
        [Inject] private GoldModel _goldModel; // ��� ��
        [Inject] private ExpModel _expModel; // ����ġ ��
        [Inject] private HpModel _hpModel; // hp ��
        [Inject] private SelectedUpgradeModel _selectedUpgradeModel; // upgrade ��

        /// UI
        [SerializeField] private GoldDropper _goldDropper;
        [Inject] private RewardViewModel _rewardViewModel;

        // Policy
        [Inject] private IGoldPolicy _goldPolicy;
        [Inject] private IHpPolicy _hpPolicy;
        [Inject] private IExpPolicy _expPolicy;
        [Inject] private ITowerPricePolicy _towerPolicy;
        
        // Service
        [Inject] private ITowerPurchaseService _towerPurchaseService;
        [Inject] private IUIFactory _uIFactory;

        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_goldDropper);
#endif


            // �ʱ�ȭ
            _mapSystem = GetComponent<MapSystem>();
            _inputSystem = GetComponent<ScreenClickInputSystem>();
            _cameraSystem = GetComponent<CameraSystem>();
            _stageSystem = GetComponent<StageSystem>();
            _waveSystem = GetComponent<WaveSystem>();
            _enemySystem = GetComponent<EnemySystem>();
            _towerSystem = GetComponent<TowerSystem>();
            _upgradeSystem = GetComponent<UpgradeSystem>();
            //////////// Input System
#if UNITY_EDITOR
            IInputStrategy inputStrategy = new PcInputStrategy();
#elif UNITY_ANDROID || UNITY_IOS
            IInputStrategy inputStrategy = new MobileInputStrategy();
#else
            IInputStrategy inputStrategy = new PcInputStrategy();
#endif
            _inputSystem.SetInputStrategy(inputStrategy);

           
            //////////// Camera System
            /// ī�޶� ������ �ʱ� ��ġ ����    
            Vector3 cameraOffset = _mapSystem.GetCenter(_mapSize.x, _mapSize.y) + new Vector3(0, 0, -10f);
            _cameraSystem.SetCameraOffset(cameraOffset);


            Vector2 maxX = GridUtility.GridToWorldPosition(_mapSize.x, _mapSize.y); // ���� �밢�� ����̿��������� ��ġ�� �޾ƿ�
            Vector2 maxY = GridUtility.GridToWorldPosition(0, _mapSize.y); // ���� �밢�� ����̿��� ������ ������ ��ġ�� �޾ƿ�
            
            
            // ī�޶� �̵� ���� ����
            _cameraSystem.SetBoundery(0, -maxY.y, maxX.x, maxY.y);

            // ī�޶� �ڵ鷯�� Input�� ���ε�
            _inputSystem.OnInputDragEvent += _cameraSystem.HandleCameraMovement;
            _inputSystem.OnCloseUpDownEvent += _cameraSystem.HandleCameraCloseUpDown;

            //////////// Stage System
            _stageSystem.OnStageStart += _waveSystem.SpawnEnemiesWave; // Stage�� ���۵Ǹ� WaveData�� �߻��ϵ��� ����

            _stageSystem.SetStageTypeStrategy(new StandardStageTypeStrategy()); // �Ϲ� ���� ���������� �����ϵ��� ����
            
            // WaveSystem
            // ���� ����Ǹ� Spawn ��ҵ� ����ǵ��� ����
            _mapSystem.OnMapChanged += () => {
                _waveSystem.SetSpawnPosition(_mapSystem.GetPath().First());
            };

            // EnemySystem
            _goldDropper.OnArrived += (enemyData) => {
                _goldModel.goldObservable.Value += _goldPolicy.CalculateKillReward(enemyData);
            };
            _mapSystem.OnMapChanged += () => {
                _gameDataHub.SetPath(_mapSystem.GetPath());
            };
            _enemySystem.OnEnemyDied += (enemyData) => { // ��� (����, �̵�) ����Ʈ
                _goldDropper.SpawnAndMoveToTarget(enemyData);
                _expModel.AddExp(_expPolicy.CalculateKillExperience(enemyData)); // ����ġ �߰�
            };
            _enemySystem.OnEnemyFinishedPath += (enemyData) => { // ������ �Ҹ�
                _hpModel.curHpObservable.Value -= _hpPolicy.CalculateHpPenaltyOnLeak(enemyData);
            };

            // ���� End ó��
            _hpModel.curHpObservable.OnValueChanged += (value) => {
                if (value <= 0) {
                    _uIFactory.InstanceUI<RewardView>(92);
                    _rewardViewModel.ProcessFinalReward();
                }
            };

            // next exp
            _expModel.levelObservable.OnValueChanged += (value) => {
                int nextLevel = _expPolicy.GetNextLevelExp(value);
                _expModel.nextExpObservable.Value = nextLevel;
            };

            // UpgradeSystem
            _expModel.levelObservable.OnValueChanged += _upgradeSystem.QueueUpgradeRequest;

            // TowerSystem
            // Ray�� Tower�� �浹������
            _inputSystem.OnRayHitEvent += _towerSystem.SelectTower;
            // Up Event�� �߻�������
            
            _inputSystem.OnUpPointEvent += _towerSystem.OnEndDrag;

            //////////// Map System
            //�� ����
            MapTema[] temas = (MapTema[])Enum.GetValues(typeof(MapTema));
            MapTema tema = temas[UnityEngine.Random.Range(0, temas.Length)];
            _mapSystem.LoadMapTema(tema);

            _mapSystem.SetPathStrategy(new SLinePathStrategy());
            _mapSystem.GenerateMap(_mapSize.x, _mapSize.y);

        }


        private void Start() {
            // �ʱ�ȭ
            
            // �ʱ� ��� ����
            _goldModel.goldObservable.Value += _goldPolicy.GetPlayerStartGold();
            // �ʱ� Ÿ�� ���� ����
            _towerPurchaseModel.towerPriceObservable.Value = _towerPolicy.GetStartPrice();

            // �ʱ� HP ����
            int startHp = _hpPolicy.GetStartPlayerHp();
            _hpModel.maxHpObservable.Value = startHp;
            _hpModel.curHpObservable.Value = startHp;

            // �ʱ� reroll Ƚ�� �߰�
            _selectedUpgradeModel.observableRerollCount.Value = 1;
        }
 


    }
}
