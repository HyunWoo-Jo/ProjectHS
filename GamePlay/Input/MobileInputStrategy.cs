using UnityEngine;
using UnityEngine.EventSystems;
using Data;
using Contracts;
namespace GamePlay
{
    public class MobileInputStrategy : InputBase {
        private Touch? _touch = null;
        private float _prevPinchDistance;   // ���� ������ �� �հ��� �Ÿ�
        private bool _isPinching;          // ��ġ ���� ������
        public override void UpdateInput() {
            //// Ŭ�� ó��
            _touch = null;
            // ��ġ �Ѱ�
            if (Input.touchCount == 1) {
                // Ŭ�� ó��
                foreach (var touch in Input.touches) { // ��ġ ��� �˻�
                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // UI Ŭ���� �ƴ� ù��° ��ġ ����� �޾ƿ�
                        _touch = touch;
                        break;
                    }
                }
                if (_touch.HasValue) {
                    Touch touch = _touch.Value;
                    if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // �����Ҷ� ��� UI Ŭ���̸� ������� ����
                        inputType = InputType.First;
                        firstFramePosition = touch.position;
                        clickStartTime = Time.time;
                        if (TryRaycastAtScreenPos(towerMask,Input.mousePosition, out RaycastHit hit)) { // Tower�ΰ� üũ
                            inputTargetType = InputTargetType.Tower;
                            hitObject = hit.collider.gameObject;
                        } else {
                            inputTargetType = InputTargetType.Ground;
                        }
                    }
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                        inputType = InputType.Push;
                    }
                    // �ش� ��ġ�� �̹� �����ӿ� �������� Ȯ�� (�հ����� �ðų� ��ҵ�)
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                        inputType = InputType.End;

                        // UI�� OnPointerUp ȣ�� �õ�
                        if (TryUIRaycast(Input.mousePosition, out RaycastResult hit)) {
                            hit.gameObject.GetComponent<IPointerUP>()?.OnPointerUP();
                        }

                    }
                } else {
                    inputType = InputType.None;
                }
            }
            //// Ȯ��, ��� ó��
            // ��ġ 2�� �̻��̸�
            if (Input.touchCount >= 2) {
                // �� �հ��� ������ ������
                Touch t0 = Input.GetTouch(0);
                Touch t1 = Input.GetTouch(1);

                float curDist = Vector2.Distance(t0.position, t1.position);
                // �ʱ� ��ġ�� ����
                if (!_isPinching || t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began) {
                    _prevPinchDistance = curDist;
                    _isPinching = true;
                } else {        // �̹� ��ġ �� �� �Ÿ� ���� ���
                    float delta = (curDist - _prevPinchDistance);
                    if (Mathf.Abs(delta) > 0.0001f)           // ������ ����
                    {
                        closeUpDownSize = delta;    // + Ȯ��, - ���
                    }
                    _prevPinchDistance = curDist;             // pre ����
                }
            } else {
                closeUpDownSize = 0;
            }
        }
        public override Vector2 GetPosition() {
            return _touch.HasValue ? _touch.Value.position : Vector2.zero;
        }
    }
}
