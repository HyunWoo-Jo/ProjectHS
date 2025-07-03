
using Zenject;
using System;
using Data;
using Contracts;
namespace UI
{
    public class PausePanelViewModel 
    {
        [Inject] WaveStatusModel _waveStatusModel;
        [Inject] private ISceneTransitionService _sts;
        public event Action<int> OnLevelEvent;

        public void UpdatePanel() {
            // WaveLevel 갱신
            OnLevelEvent?.Invoke(_waveStatusModel.waveLevelObservable.Value);
        }
        public void ChangeScene() {
            _sts.LoadScene(SceneName.MainLobbyScene);
        }

    }
} 
