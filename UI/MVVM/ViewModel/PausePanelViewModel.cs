
using Zenject;
using System;
using Data;
namespace UI
{
    public class PausePanelViewModel 
    {
        [Inject] WaveStatusModel _waveStatusModel;
        [Inject] public ILoadManager _loadManager; // Scene Load 관리 interface
        public event Action<int> OnLevelEvent;

        public void UpdatePanel() {
            // WaveLevel 갱신
            OnLevelEvent?.Invoke(_waveStatusModel.waveLevelObservable.Value);
        }
        public void ChangedScene(float delay) {
            _loadManager.LoadScene(SceneName.MainLobbyScene, delay); // 다음 씬을 호출
        }

    }
} 
