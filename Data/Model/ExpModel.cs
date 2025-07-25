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
        /// ����ġ�� ���� / ������ ����ġ�� ���� ���� ����ġ�� �ʰ��ϸ� ������
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
        /// ���� ����ġ�� ������ ���� 
        /// </summary>
        /// <param name="value"></param>
        public void SetExp(float value) {
            _expObservable.Value = Mathf.Max(0f, value); // ���� ����
        }

        public ReadOnlyReactiveProperty<float> RO_CurExpObservable => _expObservable;

        public void Notify() {
            levelObservable.ForceNotify();
            _expObservable.ForceNotify();
            nextExpObservable.ForceNotify();
        }
    }
}
