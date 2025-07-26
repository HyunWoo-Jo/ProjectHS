using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// 일반 스테이지 (모든 적을 처치 했을경우)
    /// </summary>
    public class AllEnemiesDefeatedStageEndStrategy : IStageEndStrategy {

        public bool IsComplete() {

            return false;
        }
    }
}
