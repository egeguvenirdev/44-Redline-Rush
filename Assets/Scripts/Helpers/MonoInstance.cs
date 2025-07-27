using System;
using System.Collections.Generic;
using UnityEngine;

public static class MonoInstance
{
    private static readonly Dictionary<Type, MonoBehaviour> Cache = new();
    private static readonly object _lock = new();

    public static T Get<T>(bool dontDestroyOnLoad = false) where T : MonoBehaviour
    {
        lock (_lock)
        {
            if(Cache.TryGetValue(typeof(T), out var cached) && cached !=null)
                return (T)cached;

            var instance = UnityEngine.Object.FindFirstObjectByType<T>();
            if(instance == null)
            {
                var go = new GameObject(typeof(T).Name);
                instance = go.AddComponent<T>();
            }

            if(dontDestroyOnLoad)
                UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);

            Cache[typeof(T)] = instance;
            return instance;
        }
    }

    public static bool TryCached<T> (out T instance) where T : MonoBehaviour
    {
        if(Cache.TryGetValue(typeof(T), out var cached) && cached != null)
        {
            instance = (T)cached;
            return true;
        }

        instance = null;
        return false;
    }

    public static void ClearAll()
    {
        Cache.Clear();
    }

    public static void Clear<T>() where T : MonoBehaviour
    {
        Cache.Remove(typeof(T));
    }
}