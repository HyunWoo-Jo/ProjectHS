using UnityEngine;
using System.Collections.Generic;
using Data;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEditor.Graphs;
using Unity.Mathematics;
using CustomUtility;
using ModestTree;
using System;
namespace GamePlay
{
    [DefaultExecutionOrder(80)]
    public class TowerSystem : MonoBehaviour , ITowerSystem
    {
        [Inject] private GameDataHub _gameDataHub;
        [Inject] private DataManager _dataManager;
        [Inject] private DiContainer _container;
        private Dictionary<string, GameObject> _towerPrefabDictionary = new();
        private readonly string _towerLabel = "Tower"; // Addressable Label

        [SerializeField] private float3 _towerOffset = new Vector3(0f, 0.75f, 0f);

        private List<string> _towerKeyList = new List<string> { // Addressable Key
            "ArcherTower",
            "MageTower"
        };

        private TowerBase _seletedTower;
        
        [SerializeField] private SpriteRenderer _towerShadowObjRenderer; // 타워의 그림자를 표현
        private bool _isOnShadow = false;
        
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_towerShadowObjRenderer);
#endif
            // Tower 로딩
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
        public bool TryAddTower() {
            // 비어있는 슬로 확인
            var slotList = _gameDataHub.GetSlotList();
            int index = -1;
            // 비어있는 슬롯 검색
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

            // 등록
            RegisterTowerToSlot(towerObj, index);
 
            return true;
        }

        /// <summary>
        /// 타워 위치 변경
        /// </summary>
        public void SwapTower(int index1, int index2) {
            var slotList = _gameDataHub.GetSlotList();
            // Get
            var towerData1 = slotList[index1].GetTowerData();
            var towerData2 = slotList[index2].GetTowerData();

            // Swap
            RegisterTowerToSlot(towerData1.towerObj, index2);
            RegisterTowerToSlot(towerData2.towerObj, index1);
        }

        public void RemoveTower(int index) {

        }

        // 타워를 선택 했을때 호출됨
        public void SelectTower(GameObject hitObject) {
            if (_seletedTower == null || hitObject.GetInstanceID() != _seletedTower.GetInstanceID()) {
                _seletedTower = hitObject.GetComponent<TowerBase>();
            }
            if (_seletedTower == null) return;
            // 선택한 타워 위치 변경
            UpdateDragTowerPosition();
            // shadow 표시
            UpdateTowerShadow();
        }

        // 포인터가 Up 되었을때 호출
        // 포인터 방향에서 Tower의 위치가 변경이 될 수 있도록 설정
        public void OnEndDrag() {
            if (_seletedTower != null) {
                // 배치 
                int index = PositionToIndex(_seletedTower.transform.position);
                bool isSwap = false; // swap을 했나 확인용
                if (index != -1) {
                    // 새로운 슬롯 상태 확인
                    SlotData slotData = _gameDataHub.GetSlotData(index);
                    // 이용 가능한 슬롯에, 사용중이 아닐 경우
                    if (slotData.slotState == SlotState.PlaceAble && !slotData.IsUsed()) {
                        RelocateTower(index);
                    } else if (slotData.slotState == SlotState.PlaceAble && slotData.IsUsed()) {
                        SwapTower(index, _seletedTower.index); // 타워 위치 변경
                        isSwap = true;
                    } 
                }
                if (!isSwap) _seletedTower.transform.position = IndexToTowerPosition(_seletedTower.index);
           
                // 선택 타워 정보 제거
                _seletedTower.isStop = false;
                _seletedTower = null;
                
                // 그림자 제거
                _towerShadowObjRenderer.gameObject.SetActive(false);
                _isOnShadow = false;
            }
        }
        #region private
        /// <summary>
        /// Drag 포지션 변경
        /// </summary>
        private void UpdateDragTowerPosition() {
            _seletedTower.isStop = true;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;
            _seletedTower.transform.position = newPos;
        }
        /// <summary>
        /// Tower 그림자 표시
        /// </summary>
        private void UpdateTowerShadow() {
            // shadow 표시 
            if (!_isOnShadow) {
                // Sprite 갱신
                _towerShadowObjRenderer.sprite = _seletedTower.GetTowerBaseSprite();
                _isOnShadow = true;
            }
            // 위치 정보를 index 정보로 변환
            int index = PositionToIndex(_seletedTower.transform.position);
            var slotList = _gameDataHub.GetSlotList();

            // 사용 가능한 슬롯에만 생성
            if (index != -1 && slotList[index].slotState == SlotState.PlaceAble) {
                _towerShadowObjRenderer.gameObject.SetActive(true);
                float3 shadowPos = _gameDataHub.GetIndexToWorldPosition(index) + _towerOffset;
                // 그림자 생성
                _towerShadowObjRenderer.transform.position = shadowPos;
            } else {
                _towerShadowObjRenderer.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 선택한 타워의 위치 변경
        /// </summary>
        private void RelocateTower(int index) {
            var slotList = _gameDataHub.GetSlotList();
            // 기존 슬롯 제거
            slotList[_seletedTower.index].SetTowerData(null);
            // 타워 데이터 셋
            RegisterTowerToSlot(_seletedTower, index);
        }

        /// <summary>
        /// 타워 데이터를 Slot에 등록
        /// </summary>
        private void RegisterTowerToSlot(GameObject towerObj, int index) {
            TowerBase towerBase = towerObj.GetComponent<TowerBase>();
            RegisterTowerToSlot(towerBase, index);
        }
        /// <summary>
        /// 타워 데이터를 Slot에 등록
        /// </summary>
        private void RegisterTowerToSlot(TowerBase towerBase, int index) {
            SlotData slotData = _gameDataHub.GetSlotData(index);
            // 배치
            slotData.SetTowerData(towerBase.GetTowerData());
            towerBase.transform.position = IndexToTowerPosition(index);
            towerBase.index = index;
        }


        /// <summary>
        /// Position을 index로 변환
        /// </summary>
        /// <param name="pos"></param>
        /// <returns> 실패시 -1을 반환 </returns>
        private int PositionToIndex(Vector3 pos) {
            Vector2Int grid = GridUtility.WorldToGridPosition(pos);
            return _gameDataHub.GetIndex(grid.x, grid.y);
        }

        /// <summary>
        /// Index 정보를 Tower Position으로 변경
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private float3 IndexToTowerPosition(int index) {
            return _gameDataHub.GetIndexToWorldPosition(index) + _towerOffset;
        }



        #endregion
    }
}
