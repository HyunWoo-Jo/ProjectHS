
using Zenject;
using System;
using Data;
namespace UI
{
    public class HpViewModel : IInitializable, IDisposable {
        [Inject] private HpModel _hpModel;   
        
        public event Action<int,int> OnHpChanged; // 데이터가 변경될떄 호출될 액션 (상황에 맞게 변수명을 변경해서 사용)

        public int CurHp => _hpModel.curHpObservable.Value;
        public int MaxHP => _hpModel.maxHpObservable.Value;

        /// <summary>
        /// UI 초기 갱신
        /// </summary>
        public void Update() {
            NotifyChangedHp(0);
        }

        // Zenject에서 관리
        public void Initialize() {
            _hpModel.curHpObservable.OnValueChanged += NotifyChangedHp;
            _hpModel.maxHpObservable.OnValueChanged += NotifyChangedHp;
        }
        // Zenject에서 관리
        public void Dispose() {
            _hpModel.curHpObservable.OnValueChanged -= NotifyChangedHp;
            _hpModel.maxHpObservable.OnValueChanged -= NotifyChangedHp;
        }

        /// <summary>
        /// UI 갱신 알림 매개변수는 Bind 하기위해 사용이 됨 
        /// </summary>
        /// <param name="value"></param>
        private void NotifyChangedHp(/*Observable에 Bind하기 위해 사용 되는 파라미터(사용 안함)*/int value) {
            OnHpChanged?.Invoke(CurHp, MaxHP);
        }

    }
} 
