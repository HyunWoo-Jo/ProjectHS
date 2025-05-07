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
    /// 맵 생성을 담당하는 클레스
    /// </summary>
    [DefaultExecutionOrder(80)]
    public class MapSystem : MonoBehaviour
    {
        [Inject] private DataManager _dataManager; // Addressable 데이터 관리

        private TileSpriteMapper _tileSpriteMapper; // 테마에 맞춰 Sprite를 로드
        private MapGenerator _mapGenerator;
        private bool _isLoadedTema = false; // Tema 로딩이 완료 되었나 체크 하는 변수
        private MapTema _loadedTema; // 로드한 Tema 이름
        private GameObject _fieldPrefab; // 맵 Base Prefab;
        private readonly string _prefabKey = "Field.prefab"; // Addressable Key

        // 생성된 맵 데이터
        private List<MapData> _mapDataList;
        private List<Vector3> _pathList;

        // 생성된 맵 오브젝트
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
            // 파괴될때 데이터 정리
            ReleaseTema();
            RelesePrefab();
        }

        private void Awake() {
            _tileSpriteMapper = new TileSpriteMapper(_dataManager);
            _mapGenerator = new MapGenerator(new SLinePathStrategy());
            LoadFieldPrefab();
        }

        #region public (PlayScene.cs에서 호출하는 함수)
        /// <summary>
        /// map 테마에 맞춰 Sprite를 로드 
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
        /// 생성된 맵 크기의 최대 부분을 리턴
        /// </summary>
        public Vector3 GetMax(int x, int y) {
            return _mapGenerator.MapWorldToObjectPos(x, y);
        }

        // 맵 생성
        public void GenerateMap(int sizeX, int sizeY) {
            // 맵 생성
            _mapDataList = _mapGenerator.GenerateMap(sizeX, sizeY, out _pathList);

            OnMapChanged?.Invoke();

            // Instance 맵
            StartCoroutine(InstanceMapCoroutine());
        }

        #endregion

        /// <summary>
        /// Prefab을 로드
        /// </summary>
        private void LoadFieldPrefab() {
            _dataManager.LoadAssetAsync<GameObject>(_prefabKey).ContinueWith(fieldPrefab => {
                _fieldPrefab = fieldPrefab; // 로드가 완료되면 할당
            });
        }
        /// <summary>
        /// Prefab 로드를 제거하는 함수 Destroy 에서 호출
        /// </summary>
        private void RelesePrefab() {
            _dataManager.ReleaseAsset(_prefabKey);
        }
       

        /// <summary>
        /// 데이터 정리
        /// </summary>
        private void ReleaseTema() {
            if (_isLoadedTema) { // 로드한 상태라면
                _tileSpriteMapper.ReleaseTema(_loadedTema); // 언로드
                _isLoadedTema = false;
            }
        }


        // 생성된 Data를 기반으로 Map 오브젝트 생성하는 코루틴 (GenerateMap) 에서 호출
        private IEnumerator InstanceMapCoroutine() {
            while (true) {
                if (_isLoadedTema && _mapDataList != null && _mapDataList.Count > 0 && _fieldPrefab != null) { // 로드 상태 확인
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
