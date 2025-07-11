using UnityEngine;

namespace GamePlay
{
    public interface ITowerPricePolicy
    {
        int GetCurrentPrice();       // Get Ÿ�� ����
        void AdvancePrice();         // �� �� ���� �� ���

        int GetStartPrice();
    }
}
