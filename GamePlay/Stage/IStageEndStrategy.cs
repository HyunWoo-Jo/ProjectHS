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

   
    
}
