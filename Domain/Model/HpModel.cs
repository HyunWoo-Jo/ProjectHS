using CustomUtility;
using UnityEngine;
using R3;
using Zenject;
namespace Domain
{
    public sealed class HpModel
    {
        [Inject] private IHpPolicy _hpPolicy;

        private ReactiveProperty<int> _curHpObservable = new(20);
        private ReactiveProperty<int> _maxHpObservable = new(20);


        public ReadOnlyReactiveProperty<int> RO_CurHpObservable => _curHpObservable;
        public ReadOnlyReactiveProperty<int> RO_MaxHpObservable => _maxHpObservable;

        public int CurHp {
            get { return _curHpObservable.Value; }
            private set { _curHpObservable.Value = value; }
        }
        public int MaxHp {
            get { return _maxHpObservable.Value; }
            private set { _maxHpObservable.Value = value; }
        }

        /// <summary>
        /// Hp 비율
        /// </summary>
        public float HpRatio => MaxHp <= 0 ? 0 : (float)CurHp / MaxHp;


        /// Set
        public void SetMaxHp(int value) {
            MaxHp = value;
        }
        public void SetCurHp(int value) {
            CurHp = value;
        }

        /// <summary>
        /// 최대 Hp 증가
        /// </summary>
        public void AddMaxHp(int value) {
            int add = _hpPolicy.GetAddMaxHp(value);
            MaxHp += add;
            CurHp += add;
        }


        /// <summary>
        /// 회복
        /// </summary>
        public void RecoverHp(int value) {
            CurHp += _hpPolicy.GetRecoveryHp(CurHp, MaxHp, value);
        }

        // Hp 감소
        public void ConsumeHp(int value) {
            CurHp -= _hpPolicy.GetConsumeHp(CurHp, value);
        }

        public void Notify() {
            _curHpObservable.ForceNotify();
            _maxHpObservable.ForceNotify();
        }
    }
}
