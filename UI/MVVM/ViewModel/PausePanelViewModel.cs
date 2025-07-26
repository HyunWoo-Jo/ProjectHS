
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


        public int Level => _waveStatusModel.waveLevelObservable.Value;

        public void ChangeScene() {
            _sts.LoadScene(SceneName.MainLobbyScene);
        }

    }
} 
