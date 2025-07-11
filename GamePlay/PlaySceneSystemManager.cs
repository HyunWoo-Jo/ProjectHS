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
    /// System의 제어
    /// </summary>
    [DefaultExecutionOrder(100)] // System 이 끝난 이후에 생성되도록 설정
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

        public Vector2Int _mapSize = new Vector2Int(10, 10); // 임시 맵 사이즈



        /// Model
        [Inject] private TowerPurchaseModel _towerPurchaseModel; // 타워 구매 비용 모델
        [Inject] private GoldModel _goldModel; // 골드 모델
        [Inject] private ExpModel _expModel; // 경험치 모델
        [Inject] private HpModel _hpModel; // hp 모델
        [Inject] private SelectedUpgradeModel _selectedUpgradeModel; // upgrade 모델

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


            // 초기화
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
            /// 카메라 설정과 초기 위치 조정    
            Vector3 cameraOffset = _mapSystem.GetCenter(_mapSize.x, _mapSize.y) + new Vector3(0, 0, -10f);
            _cameraSystem.SetCameraOffset(cameraOffset);


            Vector2 maxX = GridUtility.GridToWorldPosition(_mapSize.x, _mapSize.y); // 맵이 대각선 모양이여서마지막 위치를 받아옴
            Vector2 maxY = GridUtility.GridToWorldPosition(0, _mapSize.y); // 맵이 대각선 모양이여서 한줄의 마지막 위치를 받아옴
            
            
            // 카메라 이동 영역 설정
            _cameraSystem.SetBoundery(0, -maxY.y, maxX.x, maxY.y);

            // 카메라 핸들러를 Input과 바인드
            _inputSystem.OnInputDragEvent += _cameraSystem.HandleCameraMovement;
            _inputSystem.OnCloseUpDownEvent += _cameraSystem.HandleCameraCloseUpDown;

            //////////// Stage System
            _stageSystem.OnStageStart += _waveSystem.SpawnEnemiesWave; // Stage가 시작되면 WaveData를 발생하도록 설정

            _stageSystem.SetStageTypeStrategy(new StandardStageTypeStrategy()); // 일반 모드로 스테이지를 선택하도록 설정
            
            // WaveSystem
            // 맵이 변경되면 Spawn 장소도 변경되도록 변경
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
            _enemySystem.OnEnemyDied += (enemyData) => { // 골드 (생성, 이동) 이펙트
                _goldDropper.SpawnAndMoveToTarget(enemyData);
                _expModel.AddExp(_expPolicy.CalculateKillExperience(enemyData)); // 경험치 추가
            };
            _enemySystem.OnEnemyFinishedPath += (enemyData) => { // 라이프 소모
                _hpModel.curHpObservable.Value -= _hpPolicy.CalculateHpPenaltyOnLeak(enemyData);
            };

            // 게임 End 처리
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
            // Ray에 Tower가 충돌했을때
            _inputSystem.OnRayHitEvent += _towerSystem.SelectTower;
            // Up Event가 발생했을때
            
            _inputSystem.OnUpPointEvent += _towerSystem.OnEndDrag;

            //////////// Map System
            //맵 생성
            MapTema[] temas = (MapTema[])Enum.GetValues(typeof(MapTema));
            MapTema tema = temas[UnityEngine.Random.Range(0, temas.Length)];
            _mapSystem.LoadMapTema(tema);

            _mapSystem.SetPathStrategy(new SLinePathStrategy());
            _mapSystem.GenerateMap(_mapSize.x, _mapSize.y);

        }


        private void Start() {
            // 초기화
            
            // 초기 골드 설정
            _goldModel.goldObservable.Value += _goldPolicy.GetPlayerStartGold();
            // 초기 타워 가격 설정
            _towerPurchaseModel.towerPriceObservable.Value = _towerPolicy.GetStartPrice();

            // 초기 HP 설정
            int startHp = _hpPolicy.GetStartPlayerHp();
            _hpModel.maxHpObservable.Value = startHp;
            _hpModel.curHpObservable.Value = startHp;

            // 초기 reroll 횟수 추가
            _selectedUpgradeModel.observableRerollCount.Value = 1;
        }
 


    }
}
