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
        // ���� ũ�� 31 ����Ʈ
        // ũ�⸦ �� �ø� ������ �����ϸ� �и��ؼ� ����
        public float3 position; // ��ġ 
        public float speed; // �ӵ� 
        public int maxHp; // �ִ� ü�� 
        public int curHp; // ���� ü�� (���� UI, ���� �Ǻ��� ���)
        public int nextTempHp; // ������ ó�� �� �ӽ� ü�� (��ô�� ��� ������� ��� ���� �ǰ� Ȱ��) 

        public bool isDead; // ���� ����
        public bool isSpawn; // ���� ����
        public bool isObj; // Object���� ����

        public int currentPathIndex; // � Path Index��ġ�� �ֳ�
        
        
    }


}
