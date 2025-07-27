using R3;
using Zenject;
namespace Domain
{
    /// <summary>
    /// 인게임 골드 상태를 관리하는 도메인 모델
    /// </summary>
    public class GoldModel {
        [Inject] private IGoldPolicy _policy;

        private readonly ReactiveProperty<int> _goldObservable = new(20);

        public ReadOnlyReactiveProperty<int> RO_GoldObservable => _goldObservable;

        public int Gold {
            get => _goldObservable.Value;
            private set => _goldObservable.Value = value;
        }

        /// <summary>
        /// 초기 골드 설정
        /// </summary>
        public void InitializeGold(int value) {
            Gold = value;
        }

        /// <summary>
        /// 정책을 적용하여 골드 획득 시도
        /// </summary>
        public bool TryEarnGold(int earnGold) {
            if (_policy.TryEarnGold(Gold, earnGold, out var value)) {
                Gold += value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 정책을 적용하여 골드 소비 시도
        /// </summary>
        public bool TrySpendGold(int spendGold) {
            if (_policy.TrySpendGold(Gold, spendGold, out var value)) {
                Gold -= value;
                return true;
            }
            return false;
        }

        public void Notify() {
            _goldObservable.ForceNotify();
        }
    }
}
