using UnityEngine;
using System.Collections.Generic;
using Data;
using Zenject;
namespace GamePlay
{
    [DefaultExecutionOrder(80)]
    public class TowerSystem : MonoBehaviour
    {
        [Inject] private GameDataHub _gameDataHub;
        [Inject] private DataManager _dataManager;
        // tower »ý¼º
        public void AddTower() {
            

        }

        public void SwapTower(int index1, int index2) {

        }

        public void RemoveTower(int index) {

        }

    }
}
