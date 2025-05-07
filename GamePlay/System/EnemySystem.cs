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
using System.ComponentModel;
namespace GamePlay
{
    /// <summary>
    /// Enemy를 컨트롤 하는 클레스
    /// </summary>
    [BurstCompile]
    [DefaultExecutionOrder(80)]
    public class EnemySystem : MonoBehaviour {
        // DOD(data oriented design) 구조
        private NativeArray<EnemyData> _enemyDatas;
        public readonly List<ObjectPoolItem> enemyObjectPoolItemList = new();
        private NativeArray<float3> _paths;

        private void Update() {
            if (_enemyDatas.Length <= 0 || _paths.Length <= 0) return;


            /// 이동 처리
            var moveJob = new MoveJob{
                enemyDatas = _enemyDatas,
                paths = _paths,
                deltaTime = Time.deltaTime
            };

            JobHandle moveJobHandle = moveJob.Schedule(_enemyDatas.Length, 32);
            moveJobHandle.Complete(); // 완료 대기


            // Position Setting
            for (int i = 0; i < enemyObjectPoolItemList.Count; i++) {
                if (!_enemyDatas[i].isSpawn) return;
                if (_enemyDatas[i].isDead) continue;
                enemyObjectPoolItemList[i].transform.position = _enemyDatas[i].position;
                
            }
        }


        /// <summary>
        /// 이동을 관리하는 Job
        /// </summary>
        [BurstCompile]
        struct MoveJob : IJobParallelFor {
            public NativeArray<EnemyData> enemyDatas;
            [Unity.Collections.ReadOnly] public NativeArray<float3> paths;
            public float deltaTime;
            [BurstCompile]
            public void Execute(int index) {
                EnemyData curEnemyData = enemyDatas[index];
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
                    if (curEnemyData.currentPathIndex >= paths.Length) return;
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
                enemyDatas[index] = curEnemyData;
            }

        }

        /// <summary>
        /// Path가 변경되면 호출
        /// </summary>
        /// <param name="pathList"></param>
        public void SetPath(IEnumerable<Vector3> pathList) {
            if(_paths.IsCreated) _paths.Dispose();

            //// float3로 변환
            IEnumerable<float3> pathsFloat3 = pathList.Select(v => new float3(v.x, v.y, v.z));

            _paths = new NativeArray<float3>(pathsFloat3.ToArray(), Allocator.Persistent);
        }


        /// <summary>
        /// Wave System에서 할당
        /// </summary>
        /// <param name="enemyDatas"></param>
        /// <param name="enemyTrList"></param>
        public void SetEnemy(NativeArray<EnemyData> enemyDatas) {
            // enemyDatas 할당
            _enemyDatas = enemyDatas;
        }

        /// <summary>
        /// 메모리 정리
        /// </summary>
        public void Dispose() {
            _enemyDatas.Dispose(); // Native Array 정리
            enemyObjectPoolItemList.Clear();
        }
    
    }
}
