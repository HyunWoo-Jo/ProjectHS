
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

        public int GetPrice(UpgradeType type) {
            return _repo.GetPrice(type);
        }

        public int GetLevel(UpgradeType type) {
            return _repo.GetLevelLocal(type);
        }
        public int GetAbilityValue(UpgradeType type) {
            return _repo.GetAbilityValue(type);
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
