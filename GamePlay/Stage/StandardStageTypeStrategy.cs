using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// 10 �������� ���� ���� / 5�������� ���� Ÿ�̸� / ������ �Ϲ�
    /// </summary>
    public class StandardStageTypeStrategy : IStageTypeStrategy {

        public StageType GetStageType(int stageLevel) {
            if (stageLevel % 10 == 0) {
                return StageType.Boss;
            } else if (stageLevel % 5 == 0) {
                return StageType.Timer;
            } else {
                return StageType.Standard;
            }

        }
    }
}
