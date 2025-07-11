using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// ī�޶� ��� �ϴ� ��ũ��Ʈ
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class CameraSystem : MonoBehaviour
    {
        public struct CameraBoundery {
            public float maxX, maxY;
            public float minX, minY;
        }

        private Camera _camera;

        private CameraBoundery _boundery; // ī�޶� �̵� ������ ��� ����
        

        public float cameraSpeed = 400f; // ī�޶� �̵� �ӵ�

        public float cameraLerpSpeed = 2f; // ī�޶� ���� �ӵ�

        public float maxOrthographicSize = 7f;
        public float minOrthographicSize = 3f;
        public float closeUpSpeed = 1.0f;
        public Vector3 CameraOffset {  get; private set; }

        public void SetCameraOffset(Vector3 pos) {
            CameraOffset = pos;
            _camera.transform.position = pos;
        }

        /// ���� ���� ����
        public void SetBoundery(float minX, float minY, float maxX, float maxY) {
            _boundery = new CameraBoundery {
                maxX = maxX,
                maxY = maxY,
                minX = minX,
                minY = minY,
            };
        }

        private void Awake() {
            _camera = Camera.main; // main Camera�� ����
        }

        public void HandleCameraMovement(Vector2 moveDirection) {
            Vector2 direction = moveDirection.normalized; // �븻������ �ؼ� ���
            Vector3 currentPos = _camera.transform.position;
            Vector2 moveOffset = direction * cameraSpeed * Time.deltaTime;

            // ����Ʈ�� ����
            Vector3 targetPos = new Vector3(
                Mathf.Clamp(currentPos.x + moveOffset.x, _boundery.minX, _boundery.maxX),
                Mathf.Clamp(currentPos.y + moveOffset.y, _boundery.minY, _boundery.maxY),
                currentPos.z
            );
            // ����
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPos, cameraLerpSpeed * Time.deltaTime);
        }
        // ī�޶� Ȯ�����
        public void HandleCameraCloseUpDown(float value) {
            float newSize = _camera.orthographicSize - (value * closeUpSpeed * Time.deltaTime);
            _camera.orthographicSize = Mathf.Clamp(newSize, minOrthographicSize, maxOrthographicSize);
        }
    }
}
