﻿using UnityEngine;
using Data;
using Zenject;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using System;
using CustomUtility;
using UnityEngine.Assertions;
using Cysharp.Threading.Tasks;
using R3;
namespace GamePlay
{
    public abstract class TowerBase : MonoBehaviour {
        [SerializeField] protected TowerData towerData;
        [Inject] protected IEnemyDataStore enemyDataStore;
        [ReadEditor] [SerializeField] protected int targetIndex = -1;
        [SerializeField] private int price; // 판매 가격
        [SerializeField] private SpriteRenderer _towerBaseRenderer; // 본체 Renerder (그림자에 사용)
        [SerializeField] protected Animator anim;
        protected static int ShootAnimHashKey = Animator.StringToHash("Shoot"); 

        // upgrade data
        public int index;
        protected float curAttackTime = 0; // 현재 장전 시간

        public bool isStop = false;

        protected virtual void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_towerBaseRenderer);
#endif

            // Binding
            towerData.towerObj = this.gameObject;
            SetAttackSpeed(towerData.attackSpeed.Value);
            towerData.attackSpeed
                .Subscribe(SetAttackSpeed)
                .AddTo(this.gameObject);
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
            if (IsAttackAble()) { // 공격이 가능한 상태이면
                AttackLogic(); // 공격 상세 내용 호출 하위 클레스에서 구현
            }
        }
        public abstract void AttackLogic();

        public void SetAttackSpeed(float speed) { // 공격 속도 설정
            anim.speed = speed;
        }
        private bool IsAttackAble() { // 공격 가능 여부
            if (targetIndex != -1 && curAttackTime >= towerData.attackTime) return true;
            return false;
        }

       
        protected virtual void Update() {
            if (IsPause()) return;
            if (enemyDataStore.IsEnemyData()) {
                SerchIsRangeEnemiesIndex(); // 조건에 따라 매프레임 범위 안에 들어온 적 검색
                Attack(); // 공격 
            }
            UpdateAttackTimer(); // 시간 갱신
        }

        private void SerchIsRangeEnemiesIndex() { // 범위안에 들어온 적 찾기
            if (targetIndex != -1 && enemyDataStore.EnemiesLength() > targetIndex) { // 타겟이 존재하면 
                EnemyData enemyData = enemyDataStore.GetEnemyData(targetIndex);
                if (enemyData.isDead || math.distance(enemyData.position, transform.position) > towerData.range) { // 죽었거나 공격 범위를 초과했으면 초기화
                    targetIndex = -1;
                }
            }else { // 타겟이 없으면 검색 시작
                int[] temp = new int[1] { -1 };
                NativeArray<int> resultIndex = new NativeArray<int>(temp, Allocator.TempJob);
                var handle = new EnemiesSerchJob {
                    enemies = enemyDataStore.GetEnemiesData(),
                    position = transform.position,
                    range = towerData.range,
                    resultIndex = resultIndex
                }.Schedule();
                handle.Complete(); // 완료 대기
                // 범위내 존재하지 않음
                if (resultIndex[0] == -1) {
                    targetIndex = -1;
                } else {// 범위내 존재함
                    targetIndex = resultIndex[0];
                }
                resultIndex.Dispose(); // 해제
            }
        }

        [BurstCompile]
        private struct EnemiesSerchJob : IJob {
            [ReadOnly] public NativeArray<EnemyData> enemies;
            public NativeArray<int> resultIndex; // temp 결과 반환
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

        private void UpdateAttackTimer() { // 공격 시간 갱신
            curAttackTime += Time.deltaTime * towerData.attackSpeed.Value;
            if (curAttackTime > towerData.attackTime) {
                curAttackTime = towerData.attackTime;
            }
        }

    }
}
