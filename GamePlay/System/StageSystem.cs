using Data;
using System;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// Enemy 생성과 Stage 
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class StageSystem : MonoBehaviour
    {
        public event Action<StageType, int> OnStageStart; // 스테이지가 시작될때 발생되는 Event
        public event Action<int> OnStageEnd; // 스테이지가 끝날때 발생되는 Event
        public int StageLevel { get; private set; } = 1;// 현재 스테이지

        private IStageTypeStrategy _stageTypeStrategy;
        private IStageEndStrategy _stageEndStrategy;
        public StageType CurStageType { get; private set;}

        /// <summary>
        /// 스테이지 종류를 정하는 전략 설정
        /// </summary>
        /// <param name="stageTypeStrategy"></param>
        public void SetStageTypeStrategy(IStageTypeStrategy stageTypeStrategy) {
            _stageTypeStrategy = stageTypeStrategy;
        }

        /// <summary>
        /// Stage 종료 조건 설정
        /// </summary>
        /// <param name="stageType"></param>
        private void SetStageEndType(StageType stageType) {
            CurStageType = stageType;

            // 종료 조건 설정
            switch (stageType) {
                case StageType.Standard:
                _stageEndStrategy = new AllEnemiesDefeatedStageEndStrategy();
                break;
                case StageType.Boss:
                _stageEndStrategy = new BossStageEndStrategy();
                break;
                case StageType.Timer:
                _stageEndStrategy = new TimerStageEndStrategy();
                break;
            }
        }

        /// <summary>
        /// Stage가 시작 되었을때 호출
        /// </summary>
        public void StartStage() {
            if(_stageTypeStrategy == null) {
                Debug.LogError("stageTypeStrategy 전략 설정이 안되어있음");
                return;
            }
            SetStageEndType(_stageTypeStrategy.GetStageType(StageLevel)); // 종료 조건 설정
            OnStageStart?.Invoke(CurStageType, StageLevel); // Event 실행
        }
        /// <summary>
        /// Stage가 종료되었을때 호출
        /// </summary>
        private void EndStage() {


            OnStageEnd?.Invoke(StageLevel); // Event 실행
            ++StageLevel;
        }

        private void Update() {
            

        }

    }
}
