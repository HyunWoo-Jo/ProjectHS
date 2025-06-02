using Data;
using Zenject;
using System;
using System.Diagnostics;
namespace UI
{
    public class CrystalViewModel : IInitializable, IDisposable
    {
        [Inject] private ICrystalRepository _repo; // model
        public event Action<int> OnDataChanged; // 데이터가 변경될떄 호출될 액션
        public int Crystal => _repo.GetValue();

        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        private void NotifyViewDataChanged(int value) {
            OnDataChanged?.Invoke(value);
        }

        // zenject에서 관리

        public void Initialize() {
            _repo.AddChangeHandler(NotifyViewDataChanged);
 
        }

        // zenject에서 관리
        public void Dispose() {
            _repo.RemoveChangeHandler(NotifyViewDataChanged);
        }
    }
} 
