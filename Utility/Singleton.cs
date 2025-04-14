using UnityEngine;

namespace Utility
{
    [DefaultExecutionOrder(-100)]
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        private static bool _isInstance = false;
        public static T Instance {
            get {
                if (_instance == null && !_isInstance) {
                    var obj = new GameObject {
                        isStatic = true,
                        name = typeof(T).Name,
                    };
                    var t = obj.AddComponent<T>();
                    _isInstance = true;
                    return t;
                }
                return _instance;
            }
        }
        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this.GetComponent<T>();
                DontDestroyOnLoad(this);
                _isInstance = true;
            } else {
                Destroy(this.gameObject);
            }
        }

    }
}
