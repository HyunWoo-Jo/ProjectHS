using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// 스테이지 종료를 조건을 체크하는 전략
    /// </summary>
    public interface IStageEndStrategy
    {
        /// <summary>
        /// 스테이지 종료 조건이 충족되었는지 확인
        /// </summary>
        /// <returns>조건이 충족되면 true, 아니면 false</returns>
        bool IsComplete();
    }

    /// <summary>
    /// 시간
    /// </summary>
    public class TimerStageEndStrategy : IStageEndStrategy {

        public bool IsComplete() {
            return false;

        }
    }
    /// <summary>
    /// 일반 스테이지 (모든 적을 처치 했을경우)
    /// </summary>
    public class AllEnemiesDefeatedStageEndStrategy : IStageEndStrategy {

        public bool IsComplete() {

            return false;
        }
    }
    /// <summary>
    /// 보스 스테이지 
    /// </summary>
    public class BossStageEndStrategy : IStageEndStrategy {

        public bool IsComplete() {

            return false;
        }
    }
}
