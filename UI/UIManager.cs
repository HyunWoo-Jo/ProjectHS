using Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace UI {
    /// <summary>
    /// UI�� ���� �����ϴ� Ŭ����
    /// </summary>
    public class UIManager : MonoBehaviour, IUIFactory  {

        private readonly Dictionary<string, string> _keyDictionary = new(); // (class name, addressable Key)
        private readonly Dictionary<int, KeyValuePair<GameObject, string>> _objDictionary = new(); // ������ ������Ʈ�� ���� (instance Id , <Obj, Addressable key>)

        [Inject] private readonly DataManager _dataManager; // addressable Data ����
        [Inject] private UIEvent _uiEvent;
        [Inject] private DiContainer _container; // DI
       
        private IMainCanvasTag _canvasTag;

        public void SetMainCanvas(IMainCanvasTag canvasTag) {
            _canvasTag = canvasTag;
        }

        private void Awake() {
            SetAddressableKey();

            _uiEvent.OnCloseUI += CloseUI; // ������Ʈ�� �����ɶ� ���� dictionary���� ���� �ǵ��� �̺�Ʈ �߰�
        }

        /// <summary>
        /// Addressable Key�� dictionary�� ���
        /// </summary>
        private void SetAddressableKey() {
            _keyDictionary.Add(nameof(WipeUI), "Wipe_UI.prefab");

        }


        private void CloseUI(GameObject obj) {
            if (_objDictionary.TryGetValue(obj.GetInstanceID(), out var keyValue)) { // ��ϵ� ������ ���ſ� ������Ʈ �ı�
                _objDictionary.Remove(obj.GetInstanceID());
                Destroy(keyValue.Key);
                _dataManager.ReleaseAsset(keyValue.Value);
            }
        }


        /// <summary>
        /// UI ����
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

                _container.InjectGameObject(instanceObj); // ������ ������Ʈ�� Inject

                // Parent ����
                instanceObj.transform.SetParent(_canvasTag.GetRectTransform());
                instanceObj.GetComponent<Canvas>().sortingOrder = orederBy;

                

                // ���
                _objDictionary.Add(instanceObj.GetInstanceID(), new KeyValuePair<GameObject, string>(instanceObj, name));
                return instanceObj.GetComponent<T>();
            }
            return null;
        }
       
    }
}