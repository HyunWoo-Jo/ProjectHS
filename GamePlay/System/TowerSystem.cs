using UnityEngine;
using System.Collections.Generic;
using Data;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEditor.Graphs;
using Unity.Mathematics;
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
                foreach (var prefab in towerList) { // 타워 등록
                    _towerPrefabDictionary[prefab.name] = prefab; 
                }
            });
        }

        private void OnDestroy() {
            _dataManager.ReleaseAssetsByLabel(_towerLabel);
        }
        // tower 생성
        // 성공 true 실패 false
        public bool AddTower() {
            // 비어있는 슬로 ㅅ확인
            var slotList = _gameDataHub.GetSlotList();
            int index = -1;
            foreach (var slotData in slotList) {
                ++index;
                if (slotData.slotState == SlotState.PlaceAble && !slotData.IsUsed()) { // 사용 가능, 비어있는 슬롯이면
                    break;
                }
            }
            if (index == -1) {
                return false;
            }
            // Random한 Tower 생성
            string key = _towerKeyList[UnityEngine.Random.Range(0, _towerKeyList.Count)];
            GameObject towerPrefab = _towerPrefabDictionary[key];
           
            var towerObj = GameObject.Instantiate(towerPrefab); // 생성
            _container.InjectGameObject(towerObj);

            TowerBase towerBase = towerObj.GetComponent<TowerBase>();

            // 배치
            slotList[index].SetTowerData(towerBase.GetTowerData());
            towerBase.transform.position = _gameDataHub.GetGridToWorldPosition(index) + new float3(0, 0.75f, 0); // y offset

            return true;
        }

        public void SwapTower(int index1, int index2) {

        }

        public void RemoveTower(int index) {

        }

    }
}
