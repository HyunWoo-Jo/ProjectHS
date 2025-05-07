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
        public float3 position; // ��ġ
        public float speed; // �ӵ�
        public float maxHp; // �ִ� ü��
        public float curHp; // ���� ü��
        public float maxShield; // �ִ� ����
        public float curShield; // ���� ����

        public bool isDead; // ���� ����
        public bool isSpawn; // ���� ����
        public bool isObj; // Object���� ����

        public int currentPathIndex; // � Path Index��ġ�� �ֳ�
    }


}
