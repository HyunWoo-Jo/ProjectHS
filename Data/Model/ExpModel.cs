using CustomUtility;
using R3;
using System;
using UnityEngine;

namespace Data
{
    public class ExpModel 
    {
        public ReactiveProperty<int> levelObservable = new(1);

        private ReactiveProperty<float> _expObservable = new(0);
        public ReactiveProperty<float> nextExpObservable = new(10);

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

        public ReadOnlyReactiveProperty<float> RO_CurExpObservable => _expObservable;

        public void Notify() {
            levelObservable.ForceNotify();
            _expObservable.ForceNotify();
            nextExpObservable.ForceNotify();
        }
    }
}
