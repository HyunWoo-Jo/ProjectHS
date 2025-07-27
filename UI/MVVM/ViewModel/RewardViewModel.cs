
using Zenject;
using System;
using Data;
using Contracts;
namespace UI
{
    public class RewardViewModel 
    {   
        public event Action<int> OnDataChanged; // 실행되면 계산해서 반환
        [Inject] private IRewardService _rewardService;
        [Inject] private ISceneTransitionService _sts;
        public int RewardCrystal => _rewardService.CalculateRewardCrystal();

        /// <summary>
        /// 게임이 끝나면 외부에서 호출 최종 보상 처리
        /// </summary>
        public void ProcessFinalReward() {
            OnDataChanged?.Invoke(_rewardService.CalculateRewardCrystal());
            _rewardService.ProcessFinalRewards();
        }

        /// <summary>
        /// Main Lobby로 이동
        /// </summary>
        public void ChangeScene() {
            _sts.LoadScene(SceneName.MainLobbyScene);
        }

    }
} 
