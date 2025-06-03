using UnityEngine;
using Data;
using System.Linq;
using Zenject;
using System;
using GamePlay22;
using UI;
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


        private void Awake() {
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


            Vector2 maxX = _mapSystem.GetMax(_mapSize.x, _mapSize.y); // 맵이 대각선 모양이여서마지막 위치를 받아옴
            Vector2 maxY = _mapSystem.GetMax(0, _mapSize.y); // 맵이 대각선 모양이여서 한줄의 마지막 위치를 받아옴
            
            
            // 카메라 이동 영역 설정
            _cameraSystem.SetBoundery(0, -maxY.y, maxX.x, maxY.y);

            // 카메라 핸들러를 Input과 바인드
            _inputSystem.OnInputDragEvent += _cameraSystem.HandleCameraMovement;


            //////////// Stage System
            _stageSystem.OnStageStart += _waveSystem.SpawnEnemiesWave; // Stage가 시작되면 WaveData를 발생하도록 설정

            _stageSystem.SetStageTypeStrategy(new StandardStageTypeStrategy()); // 일반 모드로 스테이지를 선택하도록 설정
            
            // WaveSystem
            _mapSystem.OnMapChanged += () => {
                _waveSystem.SetSpawnPosition(_mapSystem.GetPath().First());
            };

            // EnemySystem
            _mapSystem.OnMapChanged += () => {
                _gameDataHub.SetPath(_mapSystem.GetPath());
            };


            //////////// Map System
            //맵 생성
            MapTema[] temas = (MapTema[])Enum.GetValues(typeof(MapTema));
            MapTema tema = temas[UnityEngine.Random.Range(0, temas.Length)];
            _mapSystem.LoadMapTema(tema);

            _mapSystem.SetPathStrategy(new SLinePathStrategy());
            _mapSystem.GenerateMap(_mapSize.x, _mapSize.y);


            UIInit();
        }

        private void UIInit() {
            _purchaseTowerViewModel.OnButtonClick += _towerSystem.AddTower;
        }


    }
}
