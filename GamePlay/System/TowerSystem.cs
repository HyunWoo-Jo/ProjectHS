using UnityEngine;
using System.Collections.Generic;
using Data;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEditor.Graphs;
using Unity.Mathematics;
using CustomUtility;
using ModestTree;
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

        [SerializeField] private float3 _towerOffset = new Vector3(0f, 0.75f, 0f);

        private List<string> _towerKeyList = new List<string> {
            "ArcherTower",
        };

        private TowerBase _seletedTower;
        
        [SerializeField] private SpriteRenderer _towerShadowObjRenderer; // 타워의 그림자를 표현
        private bool _isOnShadow = false;
        
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_towerShadowObjRenderer);
#endif
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
            // 비어있는 슬로 확인
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
            towerBase.transform.position = _gameDataHub.GetIndexToWorldPosition(index) + _towerOffset;
            towerBase.index = index;
            return true;
        }

        /// <summary>
        /// 타워 위치 변경
        /// </summary>
        public void SwapTower(int index1, int index2) {
            var slotList = _gameDataHub.GetSlotList();
            slotList[index1].GetTowerData(out var towerData1);
            slotList[index2].GetTowerData(out var towerData2);
            // Swap
            slotList[index1].SetTowerData(towerData2);
            slotList[index2].SetTowerData(towerData1);

            // position
            towerData1.towerObj.transform.position = towerData2.towerObj.transform.position;
            towerData2.towerObj.transform.position = _gameDataHub.GetIndexToWorldPosition(index1);
        }

        public void RemoveTower(int index) {

        }

        // 타워를 선택 했을때 호출됨
        public void SelectTower(GameObject hitObject) {
            _seletedTower = hitObject.GetComponent<TowerBase>();
            _seletedTower.isStop = true;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;
            hitObject.transform.position = newPos;
            // shadow 표시
            if (!_isOnShadow) {
                _towerShadowObjRenderer.sprite = _seletedTower.GetTowerBaseSprite();
                _isOnShadow = true;
            }
            Vector2Int grid = GridUtility.WorldToGridPosition(newPos);
            int index = _gameDataHub.GetIndex(grid.x, grid.y);
            var slotList = _gameDataHub.GetSlotList();
            if (index != -1 && slotList[index].slotState == SlotState.PlaceAble) {
                _towerShadowObjRenderer.gameObject.SetActive(true);
                float3 shadowPos = _gameDataHub.GetIndexToWorldPosition(index) + _towerOffset;
                _towerShadowObjRenderer.transform.position = shadowPos;
            } else {
                _towerShadowObjRenderer.gameObject.SetActive(false);
            }
        }

        // 포인터가 Up 되었을때 호출
        public void OnPointUp() {
            if (_seletedTower != null) {
                // 배치 
                Vector2Int grid = GridUtility.WorldToGridPosition(_seletedTower.transform.position);
                int index = _gameDataHub.GetIndex(grid.x, grid.y);
                float3 newPos = _gameDataHub.GetIndexToWorldPosition(_seletedTower.index) + _towerOffset;
                if (index != -1) {
                    // 새로운 슬롯 상태 확인
                    var slotList = _gameDataHub.GetSlotList();
                    SlotData slotData = slotList[index];
                    if (slotData.slotState == SlotState.PlaceAble && !slotData.IsUsed()) { // 이용 가능한 슬롯에, 사용중이 아닐 경우
                        newPos = _gameDataHub.GetIndexToWorldPosition(index) + _towerOffset;
                        // 인덱스 변경
                        slotList[_seletedTower.index].SetTowerData(null);
                        _seletedTower.index = index;
                        slotData.SetTowerData(_seletedTower.GetTowerData());
                    } else if (slotData.slotState == SlotState.PlaceAble && slotData.IsUsed()) {// 이용 가능한 슬롯에, 사용중
                        SwapTower(index, _seletedTower.index); // 타워 위치 변경
                    }
                }
                _seletedTower.transform.position = newPos;
                // 설정
                _seletedTower.isStop = false;
                _seletedTower = null;

                _towerShadowObjRenderer.gameObject.SetActive(false);
                _isOnShadow = false;
            }
        }
    }
}
