using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace CustomUtility { 
    public interface IObjectPool {
        internal void RepayItem(GameObject item, int index);

    }
    public static class ObjectPoolExtensions { 
        // Builder 확장 매서드
        public static ObjectPoolBuilder<T> DontDestroy<T>(this ObjectPoolBuilder<T> builder) where T : MonoBehaviour {
            GameObject.DontDestroyOnLoad(builder.pool.ownerObj);
            builder.pool.isDontDestroy = true;
            return builder;
        }
        public static ObjectPoolBuilder<T> Static<T>(this ObjectPoolBuilder<T> builder) where T : MonoBehaviour {
            builder.pool.isStatic = true;
            return builder;
        }
        public static ObjectPoolBuilder<T> AutoActivate<T>(this ObjectPoolBuilder<T> builder, bool isActivation) where T: MonoBehaviour {
            builder.pool.isAutoActivateOnBorrow = isActivation;
            return builder;
        }

        public static ObjectPool<T> Build<T>(this ObjectPoolBuilder<T> builder) where T : MonoBehaviour {
            return builder.pool;
        }

    }
    public class ObjectPoolBuilder<T> where T : MonoBehaviour {
        internal ObjectPool<T> pool;
        public static ObjectPoolBuilder<T> Instance(GameObject itemObj, int capacity = 10) {
            GameObject parentObj = new() {
                isStatic = true,
                name = itemObj.name + "_objectPool"
            };
            ObjectPoolBuilder<T> builder = new ObjectPoolBuilder<T>();

            builder.pool = new ObjectPool<T>();
            builder.pool.ownerObj = parentObj;
            builder.pool.itemObj = itemObj;
            builder.pool.item_que = new Queue<T>();
            builder.pool.index_T_list = new List<T>();
            Enumerable.Range(0, capacity).ToList().ForEach(_ => builder.pool.CreateItem());         
            return builder;
        }
    }

    public class ObjectPool<T> : IObjectPool where T : MonoBehaviour
    {
        internal GameObject ownerObj;
        internal GameObject itemObj;
        internal Queue<T> item_que;
        internal List<T> index_T_list; // Get Component를 줄이고 빠르게 반환하기위한 데이터
        public bool isStatic;
        internal bool isDontDestroy;
        internal bool isAutoActivateOnBorrow = false; // 자동 온오프 여부
        private int index = 0;
        
        // Builder를 통해 생성
        internal ObjectPool() {
        }

        public void Dipose() {
           
            itemObj = null;
            while (item_que.Count > 0) {
                T item = item_que.Dequeue();
                GameObject.DestroyImmediate(item.gameObject);
            }
            GameObject.DestroyImmediate(ownerObj);
            ownerObj = null;
        }

        internal void CreateItem() {
            GameObject obj = GameObject.Instantiate(itemObj);
            obj.AddComponent<ObjectPoolItem>().Init(this, index++);
            T t = obj.GetComponent<T>();
            item_que.Enqueue(t);
            index_T_list.Add(t);
            obj.SetActive(false);
            obj.transform.SetParent(ownerObj.transform);
            obj.isStatic = this.isStatic;
        }

        public T BorrowItem() {
            
            if (item_que.Count <= 0) {
                CreateItem();   
            }
            var item = item_que.Dequeue();
            if (isAutoActivateOnBorrow) item.gameObject.SetActive(true);
            return item;
        }

        void IObjectPool.RepayItem(GameObject item, int index) {
            if (!isStatic) {
                item.gameObject.transform.SetParent(ownerObj.transform);
                item.gameObject.SetActive(false);
            }
            item_que.Enqueue(index_T_list[index]);
        }
        public void RepayItem(T item) {
            if (!isStatic) {
                item.gameObject.transform.SetParent(ownerObj.transform);
                item.gameObject.SetActive(false);
            }
            item_que.Enqueue(item);
        }

        public IEnumerable<T> AllObjects() {
            return index_T_list;
        }

    }
}
