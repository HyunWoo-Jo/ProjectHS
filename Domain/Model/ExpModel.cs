using R3;
using UnityEngine;
using Zenject;

namespace Domain
{
    public class ExpModel 
    {
        [Inject] private IExpPolicy _expPolicy;

        private ReactiveProperty<int> _levelObservable = new(0);
        private ReactiveProperty<float> _expObservable = new(0);
        private ReactiveProperty<float> _nextExpObservable = new(10);

        public ReadOnlyReactiveProperty<int> RO_LevelObservable => _levelObservable;
        public ReadOnlyReactiveProperty<float> RO_CurExpObservable => _expObservable;
        public ReadOnlyReactiveProperty<float> RO_NextExpObservable => _nextExpObservable;

        public int Level {
            get { return _levelObservable.Value; }
            private set { _levelObservable.Value = value; }
        }
        public float CurExp {
            get { return _expObservable.Value; }
            private set { _expObservable.Value = value; }
        }

        public float NextExp {
            get { return _nextExpObservable.Value; }
            private set { _nextExpObservable.Value = value; }
        }

        public float ExpRatio => NextExp <= 0 ? 0 : (float)CurExp / NextExp;


        /// <summary>
        /// 경험치를 증가시키고 레벨업 처리
        /// </summary>
        public void AddExp(float amount) {
            if (amount <= 0f) return;

            // 정책을 통해 보정된 경험치 추가
            float gainedExp = _expPolicy.CalculateExp(amount);
            _expObservable.Value += gainedExp;

            // 레벨업 루프 처리
            while (_expObservable.Value >= _nextExpObservable.Value) {
                _expObservable.Value -= _nextExpObservable.Value;
                _levelObservable.Value++;
                _nextExpObservable.Value = _expPolicy.GetNextLevelExp(_levelObservable.Value);
            }
        }


        /// <summary>
        /// 현재 경험치를 강제로 설정 
        /// </summary>
        /// <param name="value"></param>
        public void SetExp(float value) {
            _expObservable.Value = Mathf.Max(0f, value); // 음수 방지
        }


        public void Notify() {
            _levelObservable.ForceNotify();
            _expObservable.ForceNotify();
            _nextExpObservable.ForceNotify();
        }
    }
}
