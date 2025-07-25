using System;
using UnityEngine;
using CustomUtility;
using R3;
namespace Data
{
    [Serializable]
    public class TowerData 
    {
        public GameObject towerObj;
        public int attackPower; // ���ݷ�
        public float range; // ���� ����
        public float attackTime; // ���� ����
        public ReactiveProperty<float> attackSpeed = new(1f); // �������� ������ ������ (Tower�� Anim Speed�� ���ε�)
    }
}
