using Data;
using GamePlay22;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Collections;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using System;

namespace GamePlay
{
    /// <summary>
    /// �� ������ ����ϴ� Ŭ����
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class MapSystem : MonoBehaviour
    {
        [Inject] private DataManager _dataManager; // Addressable ������ ����

        private TileSpriteMapper _tileSpriteMapper; // �׸��� ���� Sprite�� �ε�
        private MapGenerator _mapGenerator;
        private bool _isLoadedTema = false; // Tema �ε��� �Ϸ� �Ǿ��� üũ �ϴ� ����
        private MapTema _loadedTema; // �ε��� Tema �̸�
        private GameObject _fieldPrefab; // �� Base Prefab;
        private readonly string _prefabKey = "Field.prefab"; // Addressable Key

        // ������ �� ������
        private List<MapData> _mapDataList;
        private List<Vector3> _pathList;

        // ������ �� ������Ʈ
        private List<GameObject> _mapObjList;
        private GameObject _parent;

        public event Action OnMapChanged;

        public IEnumerable<MapData> GetMapData() {
            return _mapDataList;
        }

        public IEnumerable<Vector3> GetPath() {
            return _pathList;
        }

        private void OnDestroy() {
            // �ı��ɶ� ������ ����
            ReleaseTema();
            RelesePrefab();
        }

        private void Awake() {
            _tileSpriteMapper = new TileSpriteMapper(_dataManager);
            _mapGenerator = new MapGenerator(new SLinePathStrategy());
            LoadFieldPrefab();
        }

        #region public (PlayScene.cs���� ȣ���ϴ� �Լ�)
        /// <summary>
        /// map �׸��� ���� Sprite�� �ε� 
        /// </summary>
        public void LoadMapTema(MapTema tema) {
            if (_isLoadedTema) {
                ReleaseTema();
            }
            _tileSpriteMapper.LoadDataAsync(tema, () => { _isLoadedTema = true; }); 
        }
        public Vector3 GetCenter(int x, int y) {
            return _mapGenerator.MapWorldToObjectPos(x, y) * 0.5f;
        }
        /// <summary>
        /// ������ �� ũ���� �ִ� �κ��� ����
        /// </summary>
        public Vector3 GetMax(int x, int y) {
            return _mapGenerator.MapWorldToObjectPos(x, y);
        }

        // �� ����
        public void GenerateMap(int sizeX, int sizeY) {
            // �� ����
            _mapDataList = _mapGenerator.GenerateMap(sizeX, sizeY, out _pathList);

            OnMapChanged?.Invoke();

            // Instance ��
            StartCoroutine(InstanceMapCoroutine());
        }

        #endregion

        /// <summary>
        /// Prefab�� �ε�
        /// </summary>
        private void LoadFieldPrefab() {
            _dataManager.LoadAssetAsync<GameObject>(_prefabKey).ContinueWith(fieldPrefab => {
                _fieldPrefab = fieldPrefab; // �ε尡 �Ϸ�Ǹ� �Ҵ�
            });
        }
        /// <summary>
        /// Prefab �ε带 �����ϴ� �Լ� Destroy ���� ȣ��
        /// </summary>
        private void RelesePrefab() {
            _dataManager.ReleaseAsset(_prefabKey);
        }
       

        /// <summary>
        /// ������ ����
        /// </summary>
        private void ReleaseTema() {
            if (_isLoadedTema) { // �ε��� ���¶��
                _tileSpriteMapper.ReleaseTema(_loadedTema); // ��ε�
                _isLoadedTema = false;
            }
        }


        // ������ Data�� ������� Map ������Ʈ �����ϴ� �ڷ�ƾ (GenerateMap) ���� ȣ��
        private IEnumerator InstanceMapCoroutine() {
            while (true) {
                if (_isLoadedTema && _mapDataList != null && _mapDataList.Count > 0 && _fieldPrefab != null) { // �ε� ���� Ȯ��
                    _parent = new GameObject();
                    _parent.name = "Tile Parent";
                     _mapObjList = _mapGenerator.InstanceMap(_fieldPrefab, _parent.transform, _mapDataList, _tileSpriteMapper);
                    break;
                }
                yield return null;
            }
        }

    }
}
