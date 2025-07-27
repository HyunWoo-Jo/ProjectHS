using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// 스테이지 종류를 선택하는 전략
    /// </summary>
    public interface IStageTypeStrategy
    {
        StageType GetStageType(int stageLevel);
    }

}
