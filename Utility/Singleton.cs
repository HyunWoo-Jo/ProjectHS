using UnityEngine;

namespace Utility
{
   // 이제 사용 안하는 코드
   // Entenject를 사용해 생명주기 대신 관리
/*
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
*/
}
