using UnityEngine;
using Contracts;
using System;
namespace UI
{
    /// <summary>
    /// Event Trigger 객체에서 Ray 관련 호출을 할수 있도록 설정하는 클레스
    /// Input 관련 로직에서 IPointerUP을 검출하여 OnPointerUP을 호출
    /// </summary>
    public class PointerUpEventRelay : MonoBehaviour, IPointerUP
    {
        public event Action OnPointerUpEvent;
        public void OnPointerUP() {
            OnPointerUpEvent?.Invoke();
        }
    }
}
