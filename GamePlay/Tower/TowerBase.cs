using UnityEngine;
using Data;

namespace GamePlay
{
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] private float _range;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;

        public abstract void SetPosition(Vector2 pos); // ��ġ ����

        public abstract void Attack(EnemyData enemyData); // ����

        public abstract bool IsRange(); // ���� �ȿ� ���� �ֳ� üũ

        public abstract void SetAttackSpeed(float speed); // ���� �ӵ� ����

    }
}
