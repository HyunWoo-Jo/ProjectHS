using UnityEngine;
using Data;
namespace GamePlay
{
    [RequireComponent (typeof(MapSystem))]
    [RequireComponent (typeof(ScreenClickInputSystem))]
    [RequireComponent (typeof(CameraSystem))]
    public class PlayScene : MonoBehaviour
    {
        private MapSystem _mapSystem;
        private ScreenClickInputSystem _inputSystem;
        private CameraSystem _cameraSystem;

        public Vector2Int _mapSize = new Vector2Int(10, 10); // 임시 맵 사이즈
        

        private void Awake() {
            // 초기화
            _mapSystem = GetComponent<MapSystem>();
            _inputSystem = GetComponent<ScreenClickInputSystem>();
            _cameraSystem = GetComponent<CameraSystem>();

#if UNITY_EDITOR
            IInputStrategy inputStrategy = new PcInputStrategy();
#elif UNITY_ANDROID || UNITY_IOS
            IInputStrategy inputStrategy = new MobileInputStrategy();
#else
            IInputStrategy inputStrategy = new PcInputStrategy();
#endif
            _inputSystem.SetInputStrategy(inputStrategy);

            //맵 생성
            _mapSystem.LoadMapTema(MapTema.Spring);
            _mapSystem.GenerateMap(_mapSize.x, _mapSize.y);


            /// 카메라 설정과 초기 위치 조정    
            Vector3 cameraOffset = _mapSystem.GetCenter(_mapSize.x, _mapSize.y) + new Vector3(0, 0, -10f);
            _cameraSystem.SetCameraOffset(cameraOffset);


            Vector2 maxX = _mapSystem.GetMax(_mapSize.x, _mapSize.y); // 맵이 대각선 모양이여서마지막 위치를 받아옴
            Vector2 maxY = _mapSystem.GetMax(0, _mapSize.y); // 맵이 대각선 모양이여서 한줄의 마지막 위치를 받아옴
            
            // 카메라 이동 영역 설정
            _cameraSystem.SetBoundery(0, -maxY.y, maxX.x, maxY.y);

            // 카메라 핸들러를 Input과 바인드
            _inputSystem.OnInputDragEvent += _cameraSystem.HandleCameraMovement;


        }

        private void Update() {
           
        }
    }
}
