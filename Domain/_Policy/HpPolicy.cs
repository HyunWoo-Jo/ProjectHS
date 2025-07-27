using UnityEngine;
using System;
namespace Domain {

    public class HpPolicy : IHpPolicy
    {

        public int GetAddMaxHp(int value) {
            return value;
        }

        public int GetConsumeHp(int curHp, int value) {
            if (value <= 0) return 0;
            return Math.Min(value, curHp);
        }

        public int GetRecoveryHp(int curHp, int maxHp, int value) {
            if(value <= 0) return 0;
            return Math.Min(value, maxHp - curHp);
        }
    }
}
