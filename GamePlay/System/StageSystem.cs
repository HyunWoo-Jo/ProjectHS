using Data;
using System;
using UnityEngine;
using Zenject;
using Domain;
using R3;
namespace GamePlay
{
    /// <summary>
    /// Enemy 생성과 Stage 
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class StageSystem : MonoBehaviour
    {
        [Inject] private WaveStatusModel _waveStatusModel;
        [Inject] private StageSettings _stageSettings;


        public event Action<StageType, int> OnStageStart; // 스테이지가 시작될때 발생되는 Event
        public int WaveLevel => _waveStatusModel.WaveLevel;
        public float WaveTime => _waveStatusModel.WaveTime;

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
        public void StartStage(int level) {
            if(_stageTypeStrategy == null) {
                Debug.LogError("stageTypeStrategy 전략 설정이 안되어있음");
                return;
            }
            SetStageEndType(_stageTypeStrategy.GetStageType(level)); // 종료 조건 설정
            OnStageStart?.Invoke(CurStageType, level); // Event 실행
        }


        private void Start() {
            Bind();
        }

        private void Bind() {
            _waveStatusModel.RO_WaveLevelObservable
                .ThrottleLastFrame(1)
                .Subscribe(StartStage)
                .AddTo(this);
        }


        private void Update() {
            if (GameSettings.IsPause) return;
            float time = Time.deltaTime;
            _waveStatusModel.ConsumeWaveTime(time);
        }

    }
}
