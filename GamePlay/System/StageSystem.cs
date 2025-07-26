using Data;
using System;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    /// <summary>
    /// Enemy 생성과 Stage 
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class StageSystem : MonoBehaviour
    {
        [Inject] private WaveStatusModel _waveStatusModel;
        [Inject] private StageSettingsModel _stageSettingsModel;


        public event Action<StageType, int> OnStageStart; // 스테이지가 시작될때 발생되는 Event
        public event Action<int> OnStageEnd; // 스테이지가 끝날때 발생되는 Event
        public int StageLevel {
            get { return _waveStatusModel.waveLevelObservable.Value; }
            set { _waveStatusModel.waveLevelObservable.Value = value; }
        }
        public float WaveTime {
            get { return _waveStatusModel.waveTimeObservable.Value; }
            set { _waveStatusModel.waveTimeObservable.Value = value; }
        }

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
            WaveTime = _stageSettingsModel.stageDelayTime; // 남은 시간 초기화
            ++StageLevel; // 다음 스테이지
        }


        private void Start() {
            WaveTime = 0; // 기본 시간으로 설정
        }

        private void Update() {
            if (GameSettings.IsPause) return;
            float time = WaveTime;
            // 시간 계산 (추후 게임 속도, 일시정지 등이 추가 될 수 있음)
            time -= Time.deltaTime;

            WaveTime = time;
            if (time <= 0f) {
                EndStage();
                StartStage();
            }

        }

    }
}
