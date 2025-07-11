using Data;
using System;
using UnityEngine;
using Zenject;

namespace GamePlay
{
    /// <summary>
    /// Enemy ������ Stage 
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class StageSystem : MonoBehaviour
    {
        [Inject] private WaveStatusModel _waveStatusModel;
        [Inject] private StageSettingsModel _stageSettingsModel;


        public event Action<StageType, int> OnStageStart; // ���������� ���۵ɶ� �߻��Ǵ� Event
        public event Action<int> OnStageEnd; // ���������� ������ �߻��Ǵ� Event
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
        /// �������� ������ ���ϴ� ���� ����
        /// </summary>
        /// <param name="stageTypeStrategy"></param>
        public void SetStageTypeStrategy(IStageTypeStrategy stageTypeStrategy) {
            _stageTypeStrategy = stageTypeStrategy;
        }

        /// <summary>
        /// Stage ���� ���� ����
        /// </summary>
        /// <param name="stageType"></param>
        private void SetStageEndType(StageType stageType) {
            CurStageType = stageType;

            // ���� ���� ����
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
        /// Stage�� ���� �Ǿ����� ȣ��
        /// </summary>
        public void StartStage() {
            if(_stageTypeStrategy == null) {
                Debug.LogError("stageTypeStrategy ���� ������ �ȵǾ�����");
                return;
            }
            SetStageEndType(_stageTypeStrategy.GetStageType(StageLevel)); // ���� ���� ����
            OnStageStart?.Invoke(CurStageType, StageLevel); // Event ����
        }
        /// <summary>
        /// Stage�� ����Ǿ����� ȣ��
        /// </summary>
        private void EndStage() {
            OnStageEnd?.Invoke(StageLevel); // Event ����
            WaveTime = _stageSettingsModel.stageDelayTime; // ���� �ð� �ʱ�ȭ
            ++StageLevel; // ���� ��������
        }


        private void Start() {
            WaveTime = 0; // �⺻ �ð����� ����
        }

        private void Update() {
            if (GameSettings.IsPause) return;
            float time = WaveTime;
            // �ð� ��� (���� ���� �ӵ�, �Ͻ����� ���� �߰� �� �� ����)
            time -= Time.deltaTime;

            WaveTime = time;
            if (time <= 0f) {
                EndStage();
                StartStage();
            }

        }

    }
}
