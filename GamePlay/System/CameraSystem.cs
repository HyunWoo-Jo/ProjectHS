using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// 카메라 제어를 하는 스크립트
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class CameraSystem : MonoBehaviour
    {
        public struct CameraBoundery {
            public float maxX, maxY;
            public float minX, minY;
        }

        private Camera _camera;

        private CameraBoundery _boundery; // 카메라가 이동 가능한 경계 설정
        

        public float cameraSpeed = 400f; // 카메라 이동 속도

        public float cameraLerpSpeed = 2f; // 카메라 보간 속도

        public float maxOrthographicSize = 7f;
        public float minOrthographicSize = 3f;
        public float closeUpSpeed = 1.0f;
        public Vector3 CameraOffset {  get; private set; }

        public void SetCameraOffset(Vector3 pos) {
            CameraOffset = pos;
            _camera.transform.position = pos;
        }

        /// 제한 영역 설정
        public void SetBoundery(float minX, float minY, float maxX, float maxY) {
            _boundery = new CameraBoundery {
                maxX = maxX,
                maxY = maxY,
                minX = minX,
                minY = minY,
            };
        }

        private void Awake() {
            _camera = Camera.main; // main Camera로 설정
        }

        public void HandleCameraMovement(Vector2 moveDirection) {
            Vector2 direction = moveDirection.normalized; // 노말라이즈 해서 사용
            Vector3 currentPos = _camera.transform.position;
            Vector2 moveOffset = direction * cameraSpeed * Time.deltaTime;

            // 리미트값 제한
            Vector3 targetPos = new Vector3(
                Mathf.Clamp(currentPos.x + moveOffset.x, _boundery.minX, _boundery.maxX),
                Mathf.Clamp(currentPos.y + moveOffset.y, _boundery.minY, _boundery.maxY),
                currentPos.z
            );
            // 보간
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPos, cameraLerpSpeed * Time.deltaTime);
        }
        // 카메라 확대축소
        public void HandleCameraCloseUpDown(float value) {
            float newSize = _camera.orthographicSize - (value * closeUpSpeed * Time.deltaTime);
            _camera.orthographicSize = Mathf.Clamp(newSize, minOrthographicSize, maxOrthographicSize);
        }
    }
}
