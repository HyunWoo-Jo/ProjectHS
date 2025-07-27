using UnityEngine;

namespace Domain {
    public interface IHpPolicy
    {
        /// <summary>
        /// 소비 Hp
        /// </summary>
        public int GetConsumeHp(int curHp, int value);

        /// <summary>
        /// 추가 Hp
        /// </summary>
        public int GetAddMaxHp(int value);
        /// <summary>
        /// 회복 Hp
        /// </summary>
        public int GetRecoveryHp(int curHp, int maxHp, int value);
       
    }
}
