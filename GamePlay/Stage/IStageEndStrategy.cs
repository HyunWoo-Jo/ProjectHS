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

   
    
}
