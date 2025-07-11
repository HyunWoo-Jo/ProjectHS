using UnityEngine;
using Data;
namespace GamePlay
{
    /// <summary>
    /// 10 �������� ���� ���� / 5�������� ���� Ÿ�̸� / ������ �Ϲ�
    /// </summary>
    public class StandardStageTypeStrategy : IStageTypeStrategy {

        public StageType GetStageType(int stageLevel) {
            return StageType.Standard;
            /*if (stageLevel % 10 == 0) {
                return StageType.Boss;
            } else {
                return StageType.Standard;
            }*/

        }
    }
}
