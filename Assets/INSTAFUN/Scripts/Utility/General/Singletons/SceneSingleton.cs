using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        // Debug.LogError(typeof(T) + " was instantiated more than once.");
                        return _instance;
                    }

                    if (_instance != null) return _instance;
                    var singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = "[SceneSingleton] " + typeof(T);
                    // Debug.Log ("[Singleton] An instance of " + typeof(T) + " was created.");
                }

                return _instance;
            }
        }
    }
}