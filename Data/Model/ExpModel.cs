using CustomUtility;
using System;
using UnityEngine;

namespace Data
{
    public class ExpModel 
    {
        public ObservableValue<int> levelObservable = new(1);

        private ObservableValue<float> _expObservable = new(0);
        public ObservableValue<float> nextExpObservable = new(10);

        /// <summary>
        /// 경험치를 증가 / 누적된 경험치가 다음 레벨 경험치를 초과하면 레벨업
        /// </summary>
        public void AddExp(float amount) {
            if (amount <= 0f) return;

            float newExp = _expObservable.Value + amount;

            while (newExp >= nextExpObservable.Value) {
                newExp -= nextExpObservable.Value;
                levelObservable.Value += 1;
            }

            _expObservable.Value = newExp;
        }
        /// <summary>
        /// 현재 경험치를 강제로 설정 
        /// </summary>
        /// <param name="value"></param>
        public void SetExp(float value) {
            _expObservable.Value = Mathf.Max(0f, value); // 음수 방지
        }

        public float CurExp => _expObservable.Value;
        /// <summary>
        /// 경험치 변경 이벤트 구독
        /// </summary>
        public void AddExpChangedListener(Action<float> handler) {
            _expObservable.OnValueChanged += handler;
        }
        /// <summary>
        /// 경험치 변경 이벤트 해제
        /// </summary>
        public void RemoveExpChangedListener(Action<float> handler) {
            _expObservable.OnValueChanged -= handler;
        }

    }
}
