
using Zenject;
using System;
namespace UI
{
    public class PurchaseTowerViewModel 
    {   
        public event Action OnDataChanged; // 데이터가 변경될떄 호출될 액션 (상황에 맞게 변수명을 변경해서 사용)

        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        private void NotifyViewDataChanged() {
            OnDataChanged?.Invoke();
        }

    }
} 
