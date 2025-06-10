using Data;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Burst;
using CustomUtility;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Jobs;
using System.IO;
using System.Linq;
using Zenject;
using System;
using UnityEngine.Assertions;
namespace GamePlay
{
    /// <summary>
    /// Enemy를 컨트롤 하는 클레스
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class EnemySystem : MonoBehaviour {
        // DOD(data oriented design) 구조
        [Inject] private GameDataHub _gameDataHub;
        public event Action<float3> OnEnemyDied;
        public event Action OnEnemyFinishedPath;

        private void Update() {
            if (GameSettings.IsPause) return;
            var enemiesData = _gameDataHub.GetEnemiesData();
            var paths = _gameDataHub.GetPath();
            if (enemiesData.Length <= 0 || paths.Length <= 0) return;
            /// 이동 처리
            var moveJob = new MoveJob {
                enemiesData = enemiesData,
                paths = paths,
                deltaTime = Time.deltaTime
            };
            JobHandle moveJobHandle = moveJob.Schedule(enemiesData.Length, 32);
            moveJobHandle.Complete(); // 완료 대기


            // Position Setting // 죽은 적 처리
            List<ObjectPoolItem> enemyObjectPoolItemList = _gameDataHub.GetEnemyPoolList();
            for (int i = 0; i < enemyObjectPoolItemList.Count; i++) {
                var enemyData = enemiesData[i];
                if (!enemyData.isSpawn) break;
                if (enemyData.isDead || !enemyData.isObj) continue;
                if (enemyData.curHp <= 0) {
                    enemyData.isDead = true;
                    enemiesData[i] = enemyData;
                    OnEnemyDied?.Invoke(enemyData.position); // event 발생
                    continue;
                }
                if (enemyData.currentPathIndex >= paths.Length) { // 최종 경로에 도착
                    enemyData.isDead = true;
                    enemiesData[i] = enemyData;
                    OnEnemyFinishedPath?.Invoke(); // event 발생
                    continue;
                }


                enemyObjectPoolItemList[i].transform.position = enemyData.position;
            }

        }


        /// <summary>
        /// 이동을 관리하는 Job
        /// </summary>
        [BurstCompile]
        struct MoveJob : IJobParallelFor {
            public NativeArray<EnemyData> enemiesData;
            [Unity.Collections.ReadOnly] public NativeArray<float3> paths;
            public float deltaTime;
            public void Execute(int index) {
                EnemyData curEnemyData = enemiesData[index];
                if (!curEnemyData.isSpawn || curEnemyData.isDead) return; // 생성 되지 않았거나 죽었으면 연산하지 않음
                // 경로 데이터가 없거나, 현재 경로 인덱스가 유효하지 않으면 이동하지 않음
                if (paths.Length == 0 || curEnemyData.currentPathIndex < 0 || curEnemyData.currentPathIndex >= paths.Length) return;

                float3 targetPosition = paths[curEnemyData.currentPathIndex]; // 목표 지점

                // 목표 지점까지의 방향 벡터 계산
                float3 directionToTarget = targetPosition - curEnemyData.position;
                float distanceToTarget = math.length(directionToTarget);

                // 목표 지점에 거의 도달했는지 확인
                float arrivalThreshold = 0.1f;

                // 목표 지점에 도달함
                if (distanceToTarget <= arrivalThreshold) {
                    curEnemyData.currentPathIndex++;

                    // 모든 경로를 지나옴
                    if (curEnemyData.currentPathIndex >= paths.Length) {
                        enemiesData[index] = curEnemyData;
                        return;
                    }
                    // 방향 다시 계산
                    targetPosition = paths[curEnemyData.currentPathIndex];
                    directionToTarget = targetPosition - curEnemyData.position;
                    distanceToTarget = math.length(directionToTarget);
                }

                if (distanceToTarget > 0.0001f) {
                    float3 moveDirection = math.normalize(directionToTarget); // 정규화

                    // 이번 프레임에 이동할 거리 계산
                    float3 movement = moveDirection * curEnemyData.speed * deltaTime;
                    // 만약 이동 거리가 목표까지의 거리보다 크면, 목표 지점으로 바로 이동
                    if (math.lengthsq(movement) >= math.lengthsq(directionToTarget)) {
                        curEnemyData.position = targetPosition;
                    } else {
                        curEnemyData.position += movement;
                    }
                }
                enemiesData[index] = curEnemyData;
            }

        }

    }
}
