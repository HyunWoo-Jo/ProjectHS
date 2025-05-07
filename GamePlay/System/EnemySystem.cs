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
    /// Enemy�� ��Ʈ�� �ϴ� Ŭ����
    /// </summary>
    [BurstCompile]
    [DefaultExecutionOrder(80)]
    public class EnemySystem : MonoBehaviour {
        // DOD(data oriented design) ����
        private NativeArray<EnemyData> _enemyDatas;
        public readonly List<ObjectPoolItem> enemyObjectPoolItemList = new();
        private NativeArray<float3> _paths;

        private void Update() {
            if (_enemyDatas.Length <= 0 || _paths.Length <= 0) return;


            /// �̵� ó��
            var moveJob = new MoveJob{
                enemyDatas = _enemyDatas,
                paths = _paths,
                deltaTime = Time.deltaTime
            };

            JobHandle moveJobHandle = moveJob.Schedule(_enemyDatas.Length, 32);
            moveJobHandle.Complete(); // �Ϸ� ���


            // Position Setting
            for (int i = 0; i < enemyObjectPoolItemList.Count; i++) {
                if (!_enemyDatas[i].isSpawn) return;
                if (_enemyDatas[i].isDead) continue;
                enemyObjectPoolItemList[i].transform.position = _enemyDatas[i].position;
                
            }
        }


        /// <summary>
        /// �̵��� �����ϴ� Job
        /// </summary>
        [BurstCompile]
        struct MoveJob : IJobParallelFor {
            public NativeArray<EnemyData> enemyDatas;
            [Unity.Collections.ReadOnly] public NativeArray<float3> paths;
            public float deltaTime;
            [BurstCompile]
            public void Execute(int index) {
                EnemyData curEnemyData = enemyDatas[index];
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
                    if (curEnemyData.currentPathIndex >= paths.Length) return;
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
                enemyDatas[index] = curEnemyData;
            }

        }

        /// <summary>
        /// Path�� ����Ǹ� ȣ��
        /// </summary>
        /// <param name="pathList"></param>
        public void SetPath(IEnumerable<Vector3> pathList) {
            if(_paths.IsCreated) _paths.Dispose();

            //// float3�� ��ȯ
            IEnumerable<float3> pathsFloat3 = pathList.Select(v => new float3(v.x, v.y, v.z));

            _paths = new NativeArray<float3>(pathsFloat3.ToArray(), Allocator.Persistent);
        }


        /// <summary>
        /// Wave System���� �Ҵ�
        /// </summary>
        /// <param name="enemyDatas"></param>
        /// <param name="enemyTrList"></param>
        public void SetEnemy(NativeArray<EnemyData> enemyDatas) {
            // enemyDatas �Ҵ�
            _enemyDatas = enemyDatas;
        }

        /// <summary>
        /// �޸� ����
        /// </summary>
        public void Dispose() {
            _enemyDatas.Dispose(); // Native Array ����
            enemyObjectPoolItemList.Clear();
        }
    
    }
}
