using Data;
using System;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    public class ProjectileObject : MonoBehaviour
    {
        public float attackPower;
        public float speed;
        public float arrivedRange = 1f;
        public float3 _targetPos;
        private event Action _OnArrived;
       

        public void SetTarget(float3 startPos, float3 targetPos, Action arrivedEvent) {
            this.transform.position = startPos;
            _targetPos = targetPos;
            _OnArrived = arrivedEvent;

            float3 dir = math.normalize(targetPos - startPos);

            if (!math.any(math.isnan(dir))) {
                // Y축이 방향을 바라보도록 하는 회전 생성
                // forward = 고정축, up = 바라볼 방향
                quaternion rot = quaternion.LookRotationSafe(math.forward(), dir);

                transform.rotation = rot;
            }
        }

        private void Update() {
            if (GameSettings.IsPause) return;

            // 화살 이동
            float3 prevPos = (float3)transform.position;
            float3 direction = math.normalize(_targetPos - prevPos);
            float3 nextPos = prevPos + direction * speed * Time.deltaTime;

            transform.position = (Vector3)nextPos;
            

            // 도착 판정
            float distToTarget = math.distance(prevPos, _targetPos);
            float movedDist = math.distance(prevPos, nextPos);
            if (distToTarget <= movedDist + arrivedRange) {
                _OnArrived?.Invoke();
            }
        }



    }
}
