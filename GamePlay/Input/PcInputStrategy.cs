using UnityEngine;
using UnityEngine.EventSystems;
using Data;
using UI;
namespace GamePlay
{
    public class PcInputStrategy : InputBase {
        public override void UpdateInput() {

            //// Ŭ�� ó��
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) { // UI �� Ŭ���� �ƴҰ��                
                inputType = InputType.First;
                clickStartTime = Time.time;
                firstFramePosition = GetPosition();
                if (TryRaycastAtScreenPos(towerMask,Input.mousePosition, out RaycastHit hit)){ // Tower�ΰ� üũ
                    inputTargetType = InputTargetType.Tower;
                    hitObject = hit.collider.gameObject;
                } else {
                    inputTargetType = InputTargetType.Ground;
                }
            } else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) { // Push��
                inputType = InputType.Push;
            }
            if (Input.GetMouseButtonUp(0) && (inputType == InputType.First || inputType == InputType.Push)) { // UP 
                inputType = InputType.End;
                // UI�� OnPointerUp ȣ�� �õ�
                if (TryUIRaycast(Input.mousePosition, out RaycastResult hit)) {
                    hit.gameObject.GetComponent<IPointerUP>()?.OnPointerUP();
                }
            } else if (inputType == InputType.End) {
                inputType = InputType.None;

            }

            //// Ȯ��, ��� ó��
            float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
            if (Mathf.Abs(wheel) > float.Epsilon) {
                closeUpDownSize = wheel * 200f; // �ΰ���
            } else {
                closeUpDownSize = 0;
            }
            
        }

        public override Vector2 GetPosition() {
            return Input.mousePosition;
        }
    }

}
