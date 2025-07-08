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
        
        [SerializeField] private SpriteRenderer _towerShadowObjRenderer; // Ÿ���� �׸��ڸ� ǥ��
        private bool _isOnShadow = false;
        
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_towerShadowObjRenderer);
#endif
            // Tower �ε�
            _dataManager.LoadAssetsByLabelAsync<GameObject>(_towerLabel).ContinueWith(towerList => {
                foreach (var prefab in towerList) { // Ÿ�� ���
                    _towerPrefabDictionary[prefab.name] = prefab; 
                }
            });
        }

        private void OnDestroy() {
            _dataManager.ReleaseAssetsByLabel(_towerLabel);
        }
        // tower ����
        // ���� true ���� false
        public bool TryAddTower() {
            // ����ִ� ���� Ȯ��
            var slotList = _gameDataHub.GetSlotList();
            int index = -1;
            // ����ִ� ���� �˻�
            foreach (var slotData in slotList) {
                ++index;
                if (slotData.slotState == SlotState.PlaceAble && !slotData.IsUsed()) { // ��� ����, ����ִ� �����̸�
                    break;
                }
            }
            if (index == -1) {
                return false;
            }
            // Random�� Tower ����
            string key = _towerKeyList[UnityEngine.Random.Range(0, _towerKeyList.Count)];
            GameObject towerPrefab = _towerPrefabDictionary[key];
           
            var towerObj = GameObject.Instantiate(towerPrefab); // ����
            _container.InjectGameObject(towerObj);

            // ���
            RegisterTowerToSlot(towerObj, index);
 
            return true;
        }

        /// <summary>
        /// Ÿ�� ��ġ ����
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

        // Ÿ���� ���� ������ ȣ���
        public void SelectTower(GameObject hitObject) {
            if (_seletedTower == null || hitObject.GetInstanceID() != _seletedTower.GetInstanceID()) {
                _seletedTower = hitObject.GetComponent<TowerBase>();
            }
            if (_seletedTower == null) return;
            // ������ Ÿ�� ��ġ ����
            UpdateDragTowerPosition();
            // shadow ǥ��
            UpdateTowerShadow();
        }

        // �����Ͱ� Up �Ǿ����� ȣ��
        // ������ ���⿡�� Tower�� ��ġ�� ������ �� �� �ֵ��� ����
        public void OnEndDrag() {
            if (_seletedTower != null) {
                // ��ġ 
                int index = PositionToIndex(_seletedTower.transform.position);
                bool isSwap = false; // swap�� �߳� Ȯ�ο�
                if (index != -1) {
                    // ���ο� ���� ���� Ȯ��
                    SlotData slotData = _gameDataHub.GetSlotData(index);
                    // �̿� ������ ���Կ�, ������� �ƴ� ���
                    if (slotData.slotState == SlotState.PlaceAble && !slotData.IsUsed()) {
                        RelocateTower(index);
                    } else if (slotData.slotState == SlotState.PlaceAble && slotData.IsUsed()) {
                        SwapTower(index, _seletedTower.index); // Ÿ�� ��ġ ����
                        isSwap = true;
                    } 
                }
                if (!isSwap) _seletedTower.transform.position = IndexToTowerPosition(_seletedTower.index);
           
                // ���� Ÿ�� ���� ����
                _seletedTower.isStop = false;
                _seletedTower = null;
                
                // �׸��� ����
                _towerShadowObjRenderer.gameObject.SetActive(false);
                _isOnShadow = false;
            }
        }
        #region private
        /// <summary>
        /// Drag ������ ����
        /// </summary>
        private void UpdateDragTowerPosition() {
            _seletedTower.isStop = true;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;
            _seletedTower.transform.position = newPos;
        }
        /// <summary>
        /// Tower �׸��� ǥ��
        /// </summary>
        private void UpdateTowerShadow() {
            // shadow ǥ�� 
            if (!_isOnShadow) {
                // Sprite ����
                _towerShadowObjRenderer.sprite = _seletedTower.GetTowerBaseSprite();
                _isOnShadow = true;
            }
            // ��ġ ������ index ������ ��ȯ
            int index = PositionToIndex(_seletedTower.transform.position);
            var slotList = _gameDataHub.GetSlotList();

            // ��� ������ ���Կ��� ����
            if (index != -1 && slotList[index].slotState == SlotState.PlaceAble) {
                _towerShadowObjRenderer.gameObject.SetActive(true);
                float3 shadowPos = _gameDataHub.GetIndexToWorldPosition(index) + _towerOffset;
                // �׸��� ����
                _towerShadowObjRenderer.transform.position = shadowPos;
            } else {
                _towerShadowObjRenderer.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// ������ Ÿ���� ��ġ ����
        /// </summary>
        private void RelocateTower(int index) {
            var slotList = _gameDataHub.GetSlotList();
            // ���� ���� ����
            slotList[_seletedTower.index].SetTowerData(null);
            // Ÿ�� ������ ��
            RegisterTowerToSlot(_seletedTower, index);
        }

        /// <summary>
        /// Ÿ�� �����͸� Slot�� ���
        /// </summary>
        private void RegisterTowerToSlot(GameObject towerObj, int index) {
            TowerBase towerBase = towerObj.GetComponent<TowerBase>();
            RegisterTowerToSlot(towerBase, index);
        }
        /// <summary>
        /// Ÿ�� �����͸� Slot�� ���
        /// </summary>
        private void RegisterTowerToSlot(TowerBase towerBase, int index) {
            SlotData slotData = _gameDataHub.GetSlotData(index);
            // ��ġ
            slotData.SetTowerData(towerBase.GetTowerData());
            towerBase.transform.position = IndexToTowerPosition(index);
            towerBase.index = index;
        }


        /// <summary>
        /// Position�� index�� ��ȯ
        /// </summary>
        /// <param name="pos"></param>
        /// <returns> ���н� -1�� ��ȯ </returns>
        private int PositionToIndex(Vector3 pos) {
            Vector2Int grid = GridUtility.WorldToGridPosition(pos);
            return _gameDataHub.GetIndex(grid.x, grid.y);
        }

        /// <summary>
        /// Index ������ Tower Position���� ����
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private float3 IndexToTowerPosition(int index) {
            return _gameDataHub.GetIndexToWorldPosition(index) + _towerOffset;
        }



        #endregion
    }
}
