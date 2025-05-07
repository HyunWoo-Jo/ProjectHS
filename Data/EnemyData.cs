using UnityEngine;
using Unity.Mathematics;
using System;
namespace Data
{
    /// <summary>
    /// Enemy Data
    /// </summary>
    [Serializable]
    public struct EnemyData
    {
        public float3 position; // 위치
        public float speed; // 속도
        public float maxHp; // 최대 체력
        public float curHp; // 현재 체력
        public float maxShield; // 최대 쉴드
        public float curShield; // 현재 쉴드

        public bool isDead; // 죽음 여부
        public bool isSpawn; // 생성 여부
        public bool isObj; // Object존재 여부

        public int currentPathIndex; // 어떤 Path Index위치에 있나
    }


}
