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

        public Vector2Int _mapSize = new Vector2Int(10, 10); // �ӽ� �� ������
        

        private void Awake() {
            // �ʱ�ȭ
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

            //�� ����
            _mapSystem.LoadMapTema(MapTema.Spring);
            _mapSystem.GenerateMap(_mapSize.x, _mapSize.y);


            /// ī�޶� ������ �ʱ� ��ġ ����    
            Vector3 cameraOffset = _mapSystem.GetCenter(_mapSize.x, _mapSize.y) + new Vector3(0, 0, -10f);
            _cameraSystem.SetCameraOffset(cameraOffset);


            Vector2 maxX = _mapSystem.GetMax(_mapSize.x, _mapSize.y); // ���� �밢�� ����̿��������� ��ġ�� �޾ƿ�
            Vector2 maxY = _mapSystem.GetMax(0, _mapSize.y); // ���� �밢�� ����̿��� ������ ������ ��ġ�� �޾ƿ�
            
            // ī�޶� �̵� ���� ����
            _cameraSystem.SetBoundery(0, -maxY.y, maxX.x, maxY.y);

            // ī�޶� �ڵ鷯�� Input�� ���ε�
            _inputSystem.OnInputDragEvent += _cameraSystem.HandleCameraMovement;


        }

        private void Update() {
           
        }
    }
}
