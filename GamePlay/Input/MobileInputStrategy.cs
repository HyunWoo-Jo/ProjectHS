using UnityEngine;
using UnityEngine.EventSystems;
using Data;
using UI;
namespace GamePlay
{
    public class MobileInputStrategy : InputBase {
        private Touch? _touch = null;
        private float _prevPinchDistance;   // 직전 프레임 두 손가락 거리
        private bool _isPinching;          // 핀치 진행 중인지
        public override void UpdateInput() {
            //// 클릭 처리
            _touch = null;
            // 터치 한개
            if (Input.touchCount == 1) {
                // 클릭 처리
                foreach (var touch in Input.touches) { // 터치 목록 검색
                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // UI 클릭이 아닌 첫번째 터치 목록을 받아옴
                        _touch = touch;
                        break;
                    }
                }
                if (_touch.HasValue) {
                    Touch touch = _touch.Value;
                    if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId)) { // 시작할때 등록 UI 클릭이면 등록하지 않음
                        inputType = InputType.First;
                        firstFramePosition = touch.position;
                        clickStartTime = Time.time;
                        if (TryRaycastAtScreenPos(towerMask,Input.mousePosition, out RaycastHit hit)) { // Tower인가 체크
                            inputTargetType = InputTargetType.Tower;
                            hitObject = hit.collider.gameObject;
                        } else {
                            inputTargetType = InputTargetType.Ground;
                        }
                    }
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                        inputType = InputType.Push;
                    }
                    // 해당 터치가 이번 프레임에 끝났는지 확인 (손가락을 뗐거나 취소됨)
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                        inputType = InputType.End;

                        // UI면 OnPointerUp 호출 시도
                        if (TryUIRaycast(Input.mousePosition, out RaycastResult hit)) {
                            hit.gameObject.GetComponent<IPointerUP>()?.OnPointerUP();
                        }

                    }
                } else {
                    inputType = InputType.None;
                }
            }
            //// 확대, 축소 처리
            // 터치 2개 이상이면
            if (Input.touchCount >= 2) {
                // 두 손가락 정보를 가져옴
                Touch t0 = Input.GetTouch(0);
                Touch t1 = Input.GetTouch(1);

                float curDist = Vector2.Distance(t0.position, t1.position);
                // 초기 터치시 저장
                if (!_isPinching || t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began) {
                    _prevPinchDistance = curDist;
                    _isPinching = true;
                } else {        // 이미 핀치 중 → 거리 차이 계산
                    float delta = (curDist - _prevPinchDistance);
                    if (Mathf.Abs(delta) > 0.0001f)           // 노이즈 제거
                    {
                        closeUpDownSize = delta;    // + 확대, - 축소
                    }
                    _prevPinchDistance = curDist;             // pre 저장
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
