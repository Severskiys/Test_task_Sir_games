using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils.Pool
{
    public class Pool : MonoBehaviour
    {
        private static GameObject PoolGameObject { get; set; }
        private static Pool _instance;

        private static readonly Dictionary<int, Queue<IPoolable>> ItemsInQueues = new();
        private static readonly List<IPoolable> SpawnedItems = new();
        
        private static Pool Instance
        {
            get
            {
                if (_instance != default) return _instance;

                PoolGameObject = new GameObject("###_POOL_###");
                _instance = PoolGameObject.AddComponent<Pool>();

                return _instance;
            }
        }

        public static T Get<T>(T prefab) where T : Object, IPoolable
        {
            T pooledItem;
            int id = prefab.GetInstanceID();
            Queue<IPoolable> queue = GetQueue(id);
            
            if (queue.Count > 0)
            {
                pooledItem = (T)queue.Dequeue();
                pooledItem.SetActive(true);
                SpawnedItems.Add(pooledItem);
                return pooledItem;
            }
 
            pooledItem = InstantiateObject(prefab, Instance.transform, id);
            pooledItem.SetActive(true);
            SpawnedItems.Add(pooledItem);
            return pooledItem;
        }

        public static void Release<T>(int id, T poolItem) where T : Object, IPoolable
        {
            if (SpawnedItems.Contains(poolItem))
            {
                SpawnedItems.Remove(poolItem);
                var queue = GetQueue(id);
                queue.Enqueue(poolItem);
                poolItem.SetParent(Instance.transform);
                poolItem.SetActive(false);
            }
        }

        public static void ReleaseAll()
        {
            while (SpawnedItems.Count > 0)
                SpawnedItems[^1].Release();
        }

        private static Queue<IPoolable> GetQueue(int id)
        {
            if (ItemsInQueues.TryGetValue(id, out var queue)) return queue;

            queue = new Queue<IPoolable>();
            ItemsInQueues.Add(id, queue);

            return queue;
        }

        private static T InstantiateObject<T>(T prefab, Transform parent, int id) where T : Object, IPoolable
        {
            T instance = Instantiate(prefab, parent.position, Quaternion.identity, parent);
            instance.name = prefab.name;
            instance.ID = id;
            return instance;
        }
    }
}