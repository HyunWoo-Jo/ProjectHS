using System;
using UnityEngine;

namespace CustomUtility { 
    public class ObjectPoolItem : MonoBehaviour
    {
        private IObjectPool _owner;
        private int _index;

        internal ObjectPoolItem Init(IObjectPool owner, int index) {
            _owner = owner;
            _index = index;

            return this;
        }

        public void Repay() {
            _owner.RepayItem(this.gameObject, _index);
        }
    }
}
