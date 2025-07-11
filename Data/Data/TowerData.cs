using System;
using UnityEngine;
using CustomUtility;
namespace Data
{
    [Serializable]
    public class TowerData 
    {
        public GameObject towerObj;
        public int attackPower; // 공격력
        public float range; // 공격 범위
        public float attackTime; // 어택 간격
        public ObservableValue<float> attackSpeed = new(1f); // 빠를수록 공격이 빨라짐 (Tower의 Anim Speed와 바인딩)
    }
}
