using UnityEngine;
using Data;
using System.Linq;
using Zenject;
using System;
using GamePlay22;
using UI;
using UnityEngine.Assertions;
using CustomUtility;
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

        public Vector2Int _mapSize = new Vector2Int(10, 10); // 임시 맵 사이즈

        /////// viewModel
        [Inject] private PurchaseTowerViewModel _purchaseTowerViewModel;


        /// Model
        [Inject] private PurchaseTowerModel _purchaseTowerModel; // 타워 구매 비용 모델
        [Inject] private GoldModel _goldModel; // 골드 모델
        [Inject] private ExpModel _expModel; // 경험치 모델
        [Inject] private HpModel _hpModel; // hp 모델
        /// UI
        [SerializeField] private GoldDropper _goldDropper;

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
            _mapSystem.OnMapChanged += () => {
                _waveSystem.SetSpawnPosition(_mapSystem.GetPath().First());
            };

            // EnemySystem
            _goldDropper.OnArrived += () => {
                _goldModel.goldObservable.Value += 1;
                _expModel.AddExp(1);
            };
            _mapSystem.OnMapChanged += () => {
                _gameDataHub.SetPath(_mapSystem.GetPath());
            };
            _enemySystem.OnEnemyDied += (pos) => { // 골드 (생성, 이동) 이펙트
                _goldDropper.SpawnAndMoveToTarget(pos);
            };
            _enemySystem.OnEnemyFinishedPath += () => { // 라이프 소모
                _hpModel.curHpObservable.Value -= 1;
            };


            // TowerSystem
            _inputSystem.OnRayHitEvent += _towerSystem.SelectTower;
            _inputSystem.OnUpPointEvent += _towerSystem.OnPointUp;

            //////////// Map System
            //맵 생성
            MapTema[] temas = (MapTema[])Enum.GetValues(typeof(MapTema));
            MapTema tema = temas[UnityEngine.Random.Range(0, temas.Length)];
            _mapSystem.LoadMapTema(tema);

            _mapSystem.SetPathStrategy(new SLinePathStrategy());
            _mapSystem.GenerateMap(_mapSize.x, _mapSize.y);


            UIInit();

           

           
        }
        private void Start() {
            // 초기 골드 설정 (추후 업그레이드와 연결)
            _goldModel.goldObservable.Value += 10;
        }
        // ui 초기화
        private void UIInit() {
            // 구매 버튼 기능 
            _purchaseTowerViewModel.OnButtonClick += () => {
                if(_purchaseTowerModel.towerPriceObservable.Value <= _goldModel.goldObservable.Value) { // 소유한 골드가 더 많을때 
                    _goldModel.goldObservable.Value -= _purchaseTowerModel.towerPriceObservable.Value; // 골드 소모
                    _purchaseTowerModel.towerPriceObservable.Value += 1; // 타워를 구매할 때 마다 비용 증가
                    _towerSystem.AddTower();
                }
                
            };
            
        }


    }
}
