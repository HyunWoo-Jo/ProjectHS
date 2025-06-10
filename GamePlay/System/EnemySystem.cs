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
    /// Enemy�� ��Ʈ�� �ϴ� Ŭ����
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class EnemySystem : MonoBehaviour {
        // DOD(data oriented design) ����
        [Inject] private GameDataHub _gameDataHub;
        public event Action<float3> OnEnemyDied;
        public event Action OnEnemyFinishedPath;

        private void Update() {
            if (GameSettings.IsPause) return;
            var enemiesData = _gameDataHub.GetEnemiesData();
            var paths = _gameDataHub.GetPath();
            if (enemiesData.Length <= 0 || paths.Length <= 0) return;
            /// �̵� ó��
            var moveJob = new MoveJob {
                enemiesData = enemiesData,
                paths = paths,
                deltaTime = Time.deltaTime
            };
            JobHandle moveJobHandle = moveJob.Schedule(enemiesData.Length, 32);
            moveJobHandle.Complete(); // �Ϸ� ���


            // Position Setting // ���� �� ó��
            List<ObjectPoolItem> enemyObjectPoolItemList = _gameDataHub.GetEnemyPoolList();
            for (int i = 0; i < enemyObjectPoolItemList.Count; i++) {
                var enemyData = enemiesData[i];
                if (!enemyData.isSpawn) break;
                if (enemyData.isDead || !enemyData.isObj) continue;
                if (enemyData.curHp <= 0) {
                    enemyData.isDead = true;
                    enemiesData[i] = enemyData;
                    OnEnemyDied?.Invoke(enemyData.position); // event �߻�
                    continue;
                }
                if (enemyData.currentPathIndex >= paths.Length) { // ���� ��ο� ����
                    enemyData.isDead = true;
                    enemiesData[i] = enemyData;
                    OnEnemyFinishedPath?.Invoke(); // event �߻�
                    continue;
                }


                enemyObjectPoolItemList[i].transform.position = enemyData.position;
            }

        }


        /// <summary>
        /// �̵��� �����ϴ� Job
        /// </summary>
        [BurstCompile]
        struct MoveJob : IJobParallelFor {
            public NativeArray<EnemyData> enemiesData;
            [Unity.Collections.ReadOnly] public NativeArray<float3> paths;
            public float deltaTime;
            public void Execute(int index) {
                EnemyData curEnemyData = enemiesData[index];
                if (!curEnemyData.isSpawn || curEnemyData.isDead) return; // ���� ���� �ʾҰų� �׾����� �������� ����
                // ��� �����Ͱ� ���ų�, ���� ��� �ε����� ��ȿ���� ������ �̵����� ����
                if (paths.Length == 0 || curEnemyData.currentPathIndex < 0 || curEnemyData.currentPathIndex >= paths.Length) return;

                float3 targetPosition = paths[curEnemyData.currentPathIndex]; // ��ǥ ����

                // ��ǥ ���������� ���� ���� ���
                float3 directionToTarget = targetPosition - curEnemyData.position;
                float distanceToTarget = math.length(directionToTarget);

                // ��ǥ ������ ���� �����ߴ��� Ȯ��
                float arrivalThreshold = 0.1f;

                // ��ǥ ������ ������
                if (distanceToTarget <= arrivalThreshold) {
                    curEnemyData.currentPathIndex++;

                    // ��� ��θ� ������
                    if (curEnemyData.currentPathIndex >= paths.Length) {
                        enemiesData[index] = curEnemyData;
                        return;
                    }
                    // ���� �ٽ� ���
                    targetPosition = paths[curEnemyData.currentPathIndex];
                    directionToTarget = targetPosition - curEnemyData.position;
                    distanceToTarget = math.length(directionToTarget);
                }

                if (distanceToTarget > 0.0001f) {
                    float3 moveDirection = math.normalize(directionToTarget); // ����ȭ

                    // �̹� �����ӿ� �̵��� �Ÿ� ���
                    float3 movement = moveDirection * curEnemyData.speed * deltaTime;
                    // ���� �̵� �Ÿ��� ��ǥ������ �Ÿ����� ũ��, ��ǥ �������� �ٷ� �̵�
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
