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

        public Vector2Int _mapSize = new Vector2Int(10, 10); // �ӽ� �� ������

        /////// viewModel
        [Inject] private PurchaseTowerViewModel _purchaseTowerViewModel;


        /// Model
        [Inject] private PurchaseTowerModel _purchaseTowerModel; // Ÿ�� ���� ��� ��
        [Inject] private GoldModel _goldModel; // ��� ��
        [Inject] private ExpModel _expModel; // ����ġ ��
        [Inject] private HpModel _hpModel; // hp ��
        /// UI
        [SerializeField] private GoldDropper _goldDropper;

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
            _enemySystem.OnEnemyDied += (pos) => { // ��� (����, �̵�) ����Ʈ
                _goldDropper.SpawnAndMoveToTarget(pos);
            };
            _enemySystem.OnEnemyFinishedPath += () => { // ������ �Ҹ�
                _hpModel.curHpObservable.Value -= 1;
            };


            // TowerSystem
            _inputSystem.OnRayHitEvent += _towerSystem.SelectTower;
            _inputSystem.OnUpPointEvent += _towerSystem.OnPointUp;

            //////////// Map System
            //�� ����
            MapTema[] temas = (MapTema[])Enum.GetValues(typeof(MapTema));
            MapTema tema = temas[UnityEngine.Random.Range(0, temas.Length)];
            _mapSystem.LoadMapTema(tema);

            _mapSystem.SetPathStrategy(new SLinePathStrategy());
            _mapSystem.GenerateMap(_mapSize.x, _mapSize.y);


            UIInit();

           

           
        }
        private void Start() {
            // �ʱ� ��� ���� (���� ���׷��̵�� ����)
            _goldModel.goldObservable.Value += 10;
        }
        // ui �ʱ�ȭ
        private void UIInit() {
            // ���� ��ư ��� 
            _purchaseTowerViewModel.OnButtonClick += () => {
                if(_purchaseTowerModel.towerPriceObservable.Value <= _goldModel.goldObservable.Value) { // ������ ��尡 �� ������ 
                    _goldModel.goldObservable.Value -= _purchaseTowerModel.towerPriceObservable.Value; // ��� �Ҹ�
                    _purchaseTowerModel.towerPriceObservable.Value += 1; // Ÿ���� ������ �� ���� ��� ����
                    _towerSystem.AddTower();
                }
                
            };
            
        }


    }
}
