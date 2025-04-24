using Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace UI {
    /// <summary>
    /// UI를 생성 관리하는 클레스
    /// </summary>
    public class UIManager : MonoBehaviour, IUIFactory  {

        private readonly Dictionary<string, string> _keyDictionary = new();

        [Inject] private readonly DataManager _dataManager;
       
        private void Awake() {
            SetAddressableKey();
        }

        /// <summary>
        /// Addressable Key를 dictionary에 등록
        /// </summary>
        private void SetAddressableKey() {
            _keyDictionary.Add(nameof(WipeUI), "Wipe_UI.prefab");

        }

        /// <summary>
        /// UI 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="orederBy"></param>
        /// <returns></returns>
        public T InstanceUI<T>(Transform parent = null, int orederBy = 0) where T : Object {
            if(_keyDictionary.TryGetValue(typeof(T).Name, out var key)) {
                GameObject prefab = _dataManager.LoadAssetSync<GameObject>(key);
                GameObject instanceObj = Instantiate(prefab);
                if (parent != null) {
                    instanceObj.transform.SetParent(parent);
                    instanceObj.GetComponent<Canvas>().sortingOrder = orederBy;
                }
                return instanceObj.GetComponent<T>();
            }
            return null;
        }
       
    }
}