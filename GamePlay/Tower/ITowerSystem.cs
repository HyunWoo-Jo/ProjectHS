using UnityEngine;

namespace GamePlay
{
    public interface ITowerSystem
    {
        public bool TryAddTower();

        public bool TryRemoveTower(out int cost);
    }
}
