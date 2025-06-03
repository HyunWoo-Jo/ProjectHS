using UnityEngine;
using System.Collections.Generic;
using Data;
using Zenject;
using Cysharp.Threading.Tasks;
namespace GamePlay
{
    [DefaultExecutionOrder(80)]
    public class TowerSystem : MonoBehaviour
    {
        [Inject] private GameDataHub _gameDataHub;
        [Inject] private DataManager _dataManager;
        [Inject] private DiContainer _container;
        private Dictionary<string, GameObject> _towerPrefabDictionary = new();
        private readonly string _towerLabel = "Tower";
        private List<string> _towerKeyList = new List<string> {
            "ArcherTower",
        };
        
        private void Awake() {
            _dataManager.LoadAssetsByLabelAsync<GameObject>(_towerLabel).ContinueWith(towerList => {
                foreach (var prefab in towerList) { // 鸥况 殿废
                    _towerPrefabDictionary[prefab.name] = prefab; 
                }
            });
        }

        private void OnDestroy() {
            _dataManager.ReleaseAssetsByLabel(_towerLabel);
        }
        // tower 积己
        public void AddTower() {
            // Random茄 Tower 积己
            string key = _towerKeyList[Random.Range(0, _towerKeyList.Count)];
            GameObject towerPrefab = _towerPrefabDictionary[key];
           
            var towerObj = GameObject.Instantiate(towerPrefab); // 积己
            _container.InjectGameObject(towerObj);
            // 硅摹
        }

        public void SwapTower(int index1, int index2) {

        }

        public void RemoveTower(int index) {

        }

    }
}
