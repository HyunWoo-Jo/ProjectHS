using UnityEngine;
using Data;
using Zenject;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using System;
using CustomUtility;
using UnityEngine.Assertions;
namespace GamePlay
{
    public abstract class TowerBase : MonoBehaviour {
        [SerializeField] protected TowerData towerData;
        [Inject] protected IEnemyDataService enemyDataService;
        [ReadEditor] [SerializeField] protected int targetIndex = -1;
        [SerializeField] private int price; // �Ǹ� ����
        [SerializeField] private SpriteRenderer _towerBaseRenderer; // ��ü Renerder (�׸��ڿ� ���)
        [SerializeField] protected Animator anim;
        protected static int ShootAnimHashKey = Animator.StringToHash("Shoot"); 

        // upgrade data
        public int index;
        protected float curAttackTime = 0; // ���� ���� �ð�

        public bool isStop = false;

        protected virtual void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_towerBaseRenderer);
#endif

            // Binding
            towerData.towerObj = this.gameObject;
            SetAttackSpeed(towerData.attackSpeed.Value);
            towerData.attackSpeed.OnValueChanged += SetAttackSpeed;
        }

        public int Price => price;

        public Sprite GetTowerBaseSprite() => _towerBaseRenderer.sprite;
        protected bool IsPause() {
            return GameSettings.IsPause || isStop;
        }

        public TowerData GetTowerData() {
            return towerData;
        }


        public virtual void Attack() {
            if (IsAttackAble()) { // ������ ������ �����̸�
                AttackLogic(); // ���� �� ���� ȣ�� ���� Ŭ�������� ����
            }
        }
        public abstract void AttackLogic();

        public void SetAttackSpeed(float speed) { // ���� �ӵ� ����
            anim.speed = speed;
        }
        private bool IsAttackAble() { // ���� ���� ����
            if (targetIndex != -1 && curAttackTime >= towerData.attackTime) return true;
            return false;
        }

       
        protected virtual void Update() {
            if (IsPause()) return;
            if (enemyDataService.IsEnemyData()) {
                SerchIsRangeEnemiesIndex(); // ���ǿ� ���� �������� ���� �ȿ� ���� �� �˻�
                Attack(); // ���� 
            }
            UpdateAttackTimer(); // �ð� ����
        }

        private void SerchIsRangeEnemiesIndex() { // �����ȿ� ���� �� ã��
            if (targetIndex != -1 && enemyDataService.EnemiesLength() > targetIndex) { // Ÿ���� �����ϸ� 
                EnemyData enemyData = enemyDataService.GetEnemyData(targetIndex);
                if (enemyData.isDead || math.distance(enemyData.position, transform.position) > towerData.range) { // �׾��ų� ���� ������ �ʰ������� �ʱ�ȭ
                    targetIndex = -1;
                }
            }else { // Ÿ���� ������ �˻� ����
                int[] temp = new int[1] { -1 };
                NativeArray<int> resultIndex = new NativeArray<int>(temp, Allocator.TempJob);
                var handle = new EnemiesSerchJob {
                    enemies = enemyDataService.GetEnemiesData(),
                    position = transform.position,
                    range = towerData.range,
                    resultIndex = resultIndex
                }.Schedule();
                handle.Complete(); // �Ϸ� ���
                // ������ �������� ����
                if (resultIndex[0] == -1) {
                    targetIndex = -1;
                } else {// ������ ������
                    targetIndex = resultIndex[0];
                }
                resultIndex.Dispose(); // ����
            }
        }

        [BurstCompile]
        private struct EnemiesSerchJob : IJob {
            [ReadOnly] public NativeArray<EnemyData> enemies;
            public NativeArray<int> resultIndex; // temp ��� ��ȯ
            [ReadOnly] public float range;
            [ReadOnly] public float3 position;
            public void Execute() {
                for (int i = 0; i < enemies.Length; i++) {
                    if (!enemies[i].isSpawn) return;
                    if (enemies[i].isDead || enemies[i].nextTempHp <= 0) continue;
                    if (math.distance(enemies[i].position, position) <= range) {
                        resultIndex[0] = i;
                        return;
                    }
                }
            }
        }

        private void UpdateAttackTimer() { // ���� �ð� ����
            curAttackTime += Time.deltaTime * towerData.attackSpeed.Value;
            if (curAttackTime > towerData.attackTime) {
                curAttackTime = towerData.attackTime;
            }
        }

    }
}
