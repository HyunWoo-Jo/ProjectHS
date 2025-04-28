using Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace UI {
    /// <summary>
    /// UI를 생성 관리하는 클레스
    /// </summary>
    public class UIManager : MonoBehaviour, IUIFactory  {

        private readonly Dictionary<string, string> _keyDictionary = new(); // (class name, addressable Key)
        private readonly Dictionary<int, KeyValuePair<GameObject, string>> _objDictionary = new(); // 생성된 오브젝트를 관리 (instance Id , <Obj, Addressable key>)

        [Inject] private readonly DataManager _dataManager; // addressable Data 관리
        [Inject] private UIEvent _uiEvent;
        [Inject] private DiContainer _container; // DI
       
        private IMainCanvasTag _canvasTag;

        public void SetMainCanvas(IMainCanvasTag canvasTag) {
            _canvasTag = canvasTag;
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

        }


        private void CloseUI(GameObject obj) {
            if (_objDictionary.TryGetValue(obj.GetInstanceID(), out var keyValue)) { // 등록되 있으면 제거와 오브젝트 파괴
                _objDictionary.Remove(obj.GetInstanceID());
                Destroy(keyValue.Key);
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
                GameObject instanceObj = Instantiate(prefab);

                _container.InjectGameObject(instanceObj); // 생성된 오브젝트에 Inject

                // Parent 설정
                instanceObj.transform.SetParent(_canvasTag.GetRectTransform());
                instanceObj.GetComponent<Canvas>().sortingOrder = orederBy;

                

                // 등록
                _objDictionary.Add(instanceObj.GetInstanceID(), new KeyValuePair<GameObject, string>(instanceObj, name));
                return instanceObj.GetComponent<T>();
            }
            return null;
        }
       
    }
}