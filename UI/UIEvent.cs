using UnityEngine;
using System;
namespace UI
{
    /// <summary>
    /// DI���� ���� UI ���� �̺�Ʈ�� �߻��Ҷ� Ŭ����
    /// </summary>
    public class UIEvent
    {
        public event Action<GameObject> OnCloseUI; // UI�� ���� �ɶ� �߻�

        public void CloseUI(GameObject obj) {
            OnCloseUI?.Invoke(obj);
        }


    }
}
