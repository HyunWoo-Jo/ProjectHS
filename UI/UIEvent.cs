using UnityEngine;
using System;
namespace UI
{
    /// <summary>
    /// DI에서 관리 UI 관련 이벤트가 발생할때 클레스
    /// </summary>
    public class UIEvent
    {
        public event Action<GameObject> OnCloseUI; // UI가 제거 될때 발생

        public void CloseUI(GameObject obj) {
            OnCloseUI?.Invoke(obj);
        }


    }
}
