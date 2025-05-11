using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// �������� ������ �����ϴ� ����
    /// </summary>
    public interface IStageTypeStrategy
    {
        StageType GetStageType(int stageLevel);
    }

}
