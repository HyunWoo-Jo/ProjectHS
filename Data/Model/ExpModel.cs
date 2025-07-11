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

        public float CurExp => _expObservable.Value;
        /// <summary>
        /// ����ġ ���� �̺�Ʈ ����
        /// </summary>
        public void AddExpChangedListener(Action<float> handler) {
            _expObservable.OnValueChanged += handler;
        }
        /// <summary>
        /// ����ġ ���� �̺�Ʈ ����
        /// </summary>
        public void RemoveExpChangedListener(Action<float> handler) {
            _expObservable.OnValueChanged -= handler;
        }

    }
}
