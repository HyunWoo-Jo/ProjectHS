using UnityEngine;
using Data;

namespace GamePlay
{
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] private float _range;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;

        public abstract void SetPosition(Vector2 pos); // 위치 설정

        public abstract void Attack(EnemyData enemyData); // 공격

        public abstract bool IsRange(); // 범위 안에 들어와 있나 체크

        public abstract void SetAttackSpeed(float speed); // 공격 속도 설정

    }
}
