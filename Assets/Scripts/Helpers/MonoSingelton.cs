using UnityEngine;

public class MonoSingelton<T> : MonoBehaviour where T : MonoSingelton<T>
{
    private static volatile T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<T>();

            return instance;
        }
    }
}
