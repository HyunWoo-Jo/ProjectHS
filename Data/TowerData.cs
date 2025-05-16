using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class TowerData 
    {
        public float attackPower; // 공격력
        public float range; // 공격 범위
        public float attackTime; // 어택 간격
        public float attackSpeed = 1; // 빠를수록 공격이 빨라짐
    }
}
