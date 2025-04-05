using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] bool dontDestroyOnLoad;

    protected bool markedForDestroy;
    protected static T instance;

    public static T I
    {
        get
        {
            if (instance == null)
            {
                //instance = FindFirstObjectByType<T>();

                //if (instance == null)
                //{
                //    GameObject go = new GameObject();
                //    go.name = typeof(T).Name;
                //    instance = go.AddComponent<T>();
                //}
            }

            return instance;
        }
        set
        {
            if (instance == null)
                instance = value;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        // The instance was already assigned when referenced by 'I' before this awake function was called 
        else if (instance != this as T)
        {
            //Debug.LogWarning($"Already A {typeof(T).Name} in scene. Deleting this one!");
            Destroy(gameObject);
            markedForDestroy = true;
            return;
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
