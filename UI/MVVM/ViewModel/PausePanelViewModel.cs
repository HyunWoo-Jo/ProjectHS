
using Zenject;
using System;
using Data;
using Contracts;
using Domain;
namespace UI
{
    public class PausePanelViewModel 
    {
        [Inject] WaveStatusModel _waveStatusModel;
        [Inject] private ISceneTransitionService _sts;


        public int Level => _waveStatusModel.WaveLevel;

        public void ChangeScene() {
            _sts.LoadScene(SceneName.MainLobbyScene);
        }

    }
} 
