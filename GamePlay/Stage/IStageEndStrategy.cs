using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// �������� ���Ḧ ������ üũ�ϴ� ����
    /// </summary>
    public interface IStageEndStrategy
    {
        /// <summary>
        /// �������� ���� ������ �����Ǿ����� Ȯ��
        /// </summary>
        /// <returns>������ �����Ǹ� true, �ƴϸ� false</returns>
        bool IsComplete();
    }

    /// <summary>
    /// �ð�
    /// </summary>
    public class TimerStageEndStrategy : IStageEndStrategy {

        public bool IsComplete() {
            return false;

        }
    }
    /// <summary>
    /// �Ϲ� �������� (��� ���� óġ �������)
    /// </summary>
    public class AllEnemiesDefeatedStageEndStrategy : IStageEndStrategy {

        public bool IsComplete() {

            return false;
        }
    }
    /// <summary>
    /// ���� �������� 
    /// </summary>
    public class BossStageEndStrategy : IStageEndStrategy {

        public bool IsComplete() {

            return false;
        }
    }
}
