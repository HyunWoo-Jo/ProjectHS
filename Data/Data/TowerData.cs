using System;
using UnityEngine;
using CustomUtility;
namespace Data
{
    [Serializable]
    public class TowerData 
    {
        [HideInInspector] public GameObject towerObj;
        public int attackPower; // ���ݷ�
        public float range; // ���� ����
        public float attackTime; // ���� ����
        public ObservableValue<float> attackSpeed = new(1f); // �������� ������ ������ (Tower�� Anim Speed�� ���ε�)
    }
}
