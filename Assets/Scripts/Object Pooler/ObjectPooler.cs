using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        public PoolType poolType;
        public GameObject prefab;
        public int initialSize = 500;
    }

    [Header("Poolable Objects")]
    [SerializeField] private List<PoolEntry> pooleablePrefabs;

    private Dictionary<PoolType, Queue<GameObject>> poolDict = new();
    private Dictionary<PoolType, GameObject> prefabLookup = new();

    private void Awake()
    {
        foreach (var entry in pooleablePrefabs)
        {
            Queue<GameObject> queue = new();

            for (int i = 0; i < entry.initialSize; i++)
            {
                GameObject gameObject = Instantiate(entry.prefab, transform);
                gameObject.SetActive(false);
                queue.Enqueue(gameObject);
            }

            poolDict[entry.poolType] = queue;
            prefabLookup[entry.poolType] = entry.prefab;
        }
    }

    public GameObject Get(PoolType poolType, Transform targetParent, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(poolType))
        {
            Debug.LogError("Mermi Bulunamadi");
            return null;
        }

        GameObject obj = poolDict[poolType].Count > 0
            ? poolDict[poolType].Dequeue()
            : Instantiate(prefabLookup[poolType], transform);

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        obj.GetComponent<IPoolable>()?.OnTakenFromPool();
        obj.transform.parent = targetParent;
        return obj;
    }

    public GameObject Get(PoolType poolType, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(poolType))
        {
            Debug.LogError("Mermi Bulunamadi");
            return null;
        }

        GameObject obj = poolDict[poolType].Count > 0
            ? poolDict[poolType].Dequeue()
            : Instantiate(prefabLookup[poolType], transform);

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        obj.GetComponent<IPoolable>()?.OnTakenFromPool();
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            PoolType poolType = poolable.PoolType;
            obj.SetActive(false);
            obj.transform.parent = transform;
            poolable.OnReturnedToPool();
            poolDict[poolType].Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("Bu Obje poola eklenemez");
            Destroy(obj);
        }
    }
}