using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
/// such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
/// <typeparam name="T">Type of <seealso cref="PersistentSingleton{T}"/></typeparam>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Private Fields

    static T _instance;
#pragma warning disable RECS0108 // Warns about static fields in generic types
    static readonly object _lock = new object();
#pragma warning restore RECS0108 // Warns about static fields in generic types
#pragma warning disable RECS0108 // Warns about static fields in generic types
    static bool _applicationIsQuitting;
#pragma warning restore RECS0108 // Warns about static fields in generic types

    #endregion Private Fields

    #region Public Properties

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                LogManager.LogWarning(string.Format("[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning null.", typeof(T)));
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        LogManager.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        var singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = string.Format("(singleton) {0}", typeof(T));

                        DontDestroyOnLoad(singleton);

                        LogManager.Log(string.Format("[Singleton] An instance of {0} is needed in the scene, so '{1}' was created with DontDestroyOnLoad.", typeof(T), singleton));
                    }
                    else
                    {
                        LogManager.Log(string.Format("[Singleton] Using instance already created: {0}", _instance.gameObject.name));
                    }
                }

                return _instance;
            }
        }
    }

    #endregion Public Properties

    #region Private Methods

#pragma warning disable CC0091 // Use static method
#pragma warning disable CC0068 // Unused Method
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed,
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    void OnApplicationQuit()
#pragma warning restore CC0091 // Use static method
    {
        _applicationIsQuitting = true;
    }
#pragma warning restore CC0068 // Unused Method

    #endregion Private Methods
}
