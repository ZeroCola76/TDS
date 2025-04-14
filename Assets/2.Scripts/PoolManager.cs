using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private readonly Dictionary<string, ObjectPool<GameObject>> pools = new Dictionary<string, ObjectPool<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(string key, GameObject prefab, int initialSize, Transform parentTransform = null)
    {
        if (!pools.ContainsKey(key))
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab, parentTransform),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: initialSize
            );

            pools.Add(key, pool);
        }
    }

    public GameObject GetObject(string key, GameObject prefab, Transform parentTransform = null)
    {
        if (!pools.ContainsKey(key))
        {
            CreatePool(key, prefab, 50, parentTransform);
        }

        GameObject obj = pools[key].Get();

        if (parentTransform != null)
        {
            obj.transform.SetParent(parentTransform); 
        }

        return obj;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (pools.ContainsKey(key))
        {
            pools[key].Release(obj);
        }
    }

    public void InitializePools()
    {
        foreach (var pool in pools.Values)
        {
            pool.Clear();
        }
        pools.Clear();
    }
}
