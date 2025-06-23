
using Zenject;
using System;
using Data;
using System.Diagnostics;
using CustomUtility;
namespace UI
{
    public class MainLobbyUpgradeViewModel : IInitializable, IDisposable {
        [Inject] private IGlobalUpgradeRepository _repo;
        public event Action OnDataChanged; // 데이터가 변경될떄 호출될 액션 (상황에 맞게 변수명을 변경해서 사용)

        /// <summary>
        /// 데이터 변경 알림
        /// </summary>
        private void NotifyViewDataChanged() {
            OnDataChanged?.Invoke();
        }

        public int GetData(UpgradeType type) {
            return _repo.GetValueLocal(type);
        }

        public GlobalUpgradeDataSO GetUpgradeDataSO() {
            return _repo.GetUpgradeTable();
        }

        // Zenject에서 관리
        public void Initialize() {
            _repo.AddChangedHandler(NotifyViewDataChanged);
        }
        // Zenject에서 관리
        public void Dispose() {
            _repo.AddChangedHandler(NotifyViewDataChanged);
        }
    }
} 
