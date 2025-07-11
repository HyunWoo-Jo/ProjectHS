using UnityEngine;
using Contracts;
using System;
namespace UI
{
    /// <summary>
    /// Event Trigger ��ü���� Ray ���� ȣ���� �Ҽ� �ֵ��� �����ϴ� Ŭ����
    /// Input ���� �������� IPointerUP�� �����Ͽ� OnPointerUP�� ȣ��
    /// </summary>
    public class PointerUpEventRelay : MonoBehaviour, IPointerUP
    {
        public event Action OnPointerUpEvent;
        public void OnPointerUP() {
            OnPointerUpEvent?.Invoke();
        }
    }
}
