using System;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    public class ProjectileObject : MonoBehaviour
    {
        public float attackPower;
        public float speed;
        public float arrivedRange = 1f;
        public float3 _targetPos;
        private event Action _OnArrived;
        

        public void SetTarget(float3 targetPos, Action arrivedEvent) {
            _targetPos = targetPos;
            _OnArrived = arrivedEvent;
        }

        private void Update() {
            // 화살 이동
            float3 prevPos = (float3)transform.position;
            float3 direction = math.normalize(_targetPos - prevPos);
            float3 nextPos = prevPos + direction * speed * Time.deltaTime;

            transform.position = (Vector3)nextPos;
            transform.LookAt(_targetPos);

            // 도착 판정
            float distToTarget = math.distance(prevPos, _targetPos);
            float movedDist = math.distance(prevPos, nextPos);
            if (distToTarget <= movedDist + arrivedRange) {
                _OnArrived?.Invoke();
            }
        }



    }
}
