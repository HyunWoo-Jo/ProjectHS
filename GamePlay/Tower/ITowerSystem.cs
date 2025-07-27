using UnityEngine;

namespace GamePlay
{
    public interface ITowerSystem
    {
        /// <summary>
        /// 비어있는 슬롯 검색 
        /// </summary>
        /// <returns> -1 실패</returns>
        int SerchEmptySlot(); 
        void AddTower(int index);

        bool TryRemoveTower(out int cost);
    }
}
