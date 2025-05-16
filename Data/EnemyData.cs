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
        // 현재 크기 39 바이트
        // 크기를 더 늘릴 생각이 존재하면 분리해서 저장
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
