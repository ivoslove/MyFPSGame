using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>
{
    private static T _instance;

    public T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Activator.CreateInstance<T>();
            }

            return _instance;
        }
    }

    protected Singleton()
    {
        Init();
    }

    protected virtual void Init(){}

}

public abstract  class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static bool _bIsDestroy = false;
    private static T _instance = null;
    void Awake()
    {
        AwakeHandle();
    }

    public static  T Instance
    {
        get
        {
            if (_instance == null && !_bIsDestroy)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj=new GameObject(typeof(T).Name,typeof(T));
                    _instance = obj.GetComponent<T>();
                }
                _bIsDestroy = false;
            }

            return _instance;
        }
    }

    protected virtual void AwakeHandle()
    {
        Init();
    }
    protected virtual void Init() { }
    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
        //_instance = null;
    }
    protected virtual void OnApplicationQuit()
    {
        _bIsDestroy = true;
    }
    public virtual void InstantiateMonoSingleton(params object[] args) { }
}
