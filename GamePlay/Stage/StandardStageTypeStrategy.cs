using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// 10 스테이지 간격 보스 / 5스테이지 간격 타이머 / 나머지 일반
    /// </summary>
    public class StandardStageTypeStrategy : IStageTypeStrategy {

        public StageType GetStageType(int stageLevel) {
            if (stageLevel % 10 == 0) {
                return StageType.Boss;
            } else {
                return StageType.Standard;
            }

        }
    }
}
