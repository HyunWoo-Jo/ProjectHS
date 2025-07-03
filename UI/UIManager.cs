using Data;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;


namespace UI {
    /// <summary>
    /// UI를 생성 관리하는 클레스
    /// </summary>
    public class UIManager : MonoBehaviour, IUIFactory, ISetMainCanvas  {

        private readonly Dictionary<string, string> _keyDictionary = new(); // (class name, addressable Key)
        private readonly Dictionary<int, KeyValuePair<GameObject, string>> _objDictionary = new(); // 생성된 오브젝트를 관리 (instance Id , <Obj, Addressable key>)

        [Inject] private readonly DataManager _dataManager; // addressable Data 관리
        [Inject] private UIEvent _uiEvent;
        private DiContainer _container; // DI

        private IMainCanvasTag _canvasTag;

        public void SetMainCanvas(IMainCanvasTag canvasTag, DiContainer container) {
            _canvasTag = canvasTag;
            _container = container;
        }

        private void Awake() {
            SetAddressableKey();

            _uiEvent.OnCloseUI += CloseUI; // 오브젝트가 삭제될때 관리 dictionary에서 제거 되도록 이벤트 추가

        }


        /// <summary>
        /// Addressable Key를 dictionary에 등록
        /// </summary>
        private void SetAddressableKey() {
            _keyDictionary.Add(nameof(WipeUI), "Wipe_UI.prefab");
            _keyDictionary.Add(nameof(UpgradeView), "Upgrade_SubCanvas.prefab");
            _keyDictionary.Add(nameof(PausePanelView), "PausePanel_UI_Canvas.prefab");
            _keyDictionary.Add(nameof(RewardView), "RewardPanel_UI_Canvas.prefab");

        }


        private void CloseUI(GameObject obj) {
            if (_objDictionary.TryGetValue(obj.GetInstanceID(), out var keyValue)) { // 등록되 있으면 제거와 오브젝트 파괴
                _objDictionary.Remove(obj.GetInstanceID());
                _dataManager.ReleaseAsset(keyValue.Value);
            }
        }


        /// <summary>
        /// UI 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="orederBy"></param>
        /// <returns></returns>
        public T InstanceUI<T>(int orederBy = 0) where T : Object {
            string name = typeof(T).Name;
            if (_keyDictionary.TryGetValue(name, out var key)) {
                GameObject prefab = _dataManager.LoadAssetSync<GameObject>(key);
               
                GameObject instanceObj = _container.InstantiatePrefab(prefab); // inject 동시에 생성
                // Parent 설정
                instanceObj.transform.SetParent(_canvasTag.GetRectTransform());
                Canvas canvas = instanceObj.GetComponent<Canvas>();
                if(canvas != null) {
                    canvas.sortingOrder = orederBy;
                    // canvas 이면 위치 중앙으로 고정
                    var rt = canvas.GetComponent<RectTransform>();
                    rt.offsetMin = Vector2.zero; // Left, Bottom = 0
                    rt.offsetMax = Vector2.zero; // Right, Top = 0
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.one;
                }

                // 등록
                _objDictionary.Add(instanceObj.GetInstanceID(), new KeyValuePair<GameObject, string>(instanceObj, name));
                return instanceObj.GetComponent<T>();
            }
            return null;
        }
       
    }
}