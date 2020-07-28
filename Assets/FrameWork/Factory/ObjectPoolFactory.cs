using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolFactory<T> where T:new()
{
    private static Dictionary<string, Pool<T>> poolDic=new Dictionary<string, Pool<T>>();
    public static T GetObject()
    {
        string id = typeof(T).GetHashCode().ToString();

        if (poolDic.ContainsKey(id))
        {
            var pool = poolDic[id];
            return pool.GetObj();
        }
        else
        {
            Pool<T> pool = new Pool<T>();
           poolDic.Add(id,pool);
           T t = pool.GetObj();
           return t;
        }
        
    
    }

    public static void Collect(T t)
    {
        string id = typeof(T).GetHashCode().ToString();
        if (!poolDic.ContainsKey(id))
        {
            Debug.LogError("竟然没有" + typeof(T).Name+ "的对象池");
        }
        else
        {
            var pool = poolDic[id];
            pool.Collect(t);
        }
    }
}

public  class Pool<T> where T : new()
{
    private Stack<T> pool;

    protected string id;

    public Pool()
    {
        id = typeof(T).GetHashCode().ToString();
        pool=new Stack<T>();
    }

    public T GetObj()
    {
    
        if (pool.Count>0)
        {
            T t = pool.Pop();
            return t;
        }
        else
        {
            T t = new T();
            return t;
        }
    }

    public void Collect(T t)
    {
        pool.Push(t);
    }

}

/// <summary>
/// 开火后跳出来的空弹壳
/// </summary>
public class GlockEmptyCasePool:Pool<GlockEmptyCase>
{
    public GlockEmptyCasePool()
    {
        string id = typeof(GlockEmptyCase).GetHashCode().ToString();
    }
}

public class GlockEmptyCase
{
    public GameObject obj;

    public GlockEmptyCase()
    {
        GameObject model = Resources.Load<GameObject>("Prefab/Weapon/BulletCase");
        this.obj = GameObject.Instantiate(model);
    }
}
public class AKMEmptyCasePool : Pool<AKMEmptyCase>
{
    public AKMEmptyCasePool()
    {
        string id = typeof(AKMEmptyCase).GetHashCode().ToString();
    }
}

public class AKMEmptyCase
{
    public GameObject obj;

    public AKMEmptyCase()
    {
        GameObject model = Resources.Load<GameObject>("Prefab/Weapon/BulletCase");
        this.obj = GameObject.Instantiate(model);
    }
}


/// <summary>
/// MP5Pool
/// </summary>
public class MP5EmptyCasePool : Pool<MP5EmptyCase>
{
    public MP5EmptyCasePool()
    {
        string id = typeof(MP5EmptyCase).GetHashCode().ToString();
    }
}

public class MP5EmptyCase
{
    public GameObject obj;

    public MP5EmptyCase()
    {
        GameObject model = Resources.Load<GameObject>("Prefab/Weapon/BulletCase");
        this.obj = GameObject.Instantiate(model);
    }
}

public class M870EmptyCasePool : Pool<M870EmptyCase>
{
    public M870EmptyCasePool()
    {
        string id = typeof(M870EmptyCase).GetHashCode().ToString();
    }
}

public class M870EmptyCase
{
    public GameObject obj;

    public M870EmptyCase()
    {
        GameObject model = Resources.Load<GameObject>("Prefab/Weapon/BulletCase");
        this.obj = GameObject.Instantiate(model);
    }
}
/// <summary>
/// 子弹痕迹效果池
/// </summary>
public class BulletImpactPool : Pool<BulletImpact>
{
    public BulletImpactPool()
    {
        string id = typeof(BulletImpact).GetHashCode().ToString();
    }
}

public class BulletImpact
{
    public GameObject obj;
    public BulletImpact()
    {
        GameObject model = Resources.Load<GameObject>("Prefab/BulletHoleAssets/BulletImpact");
        this.obj = GameObject.Instantiate(model);
    }
}

public class FireHitParticle
{
    public GameObject obj;

    public FireHitParticle()
    {
        GameObject model = Resources.Load<GameObject>("Prefab/Weapon/HitSparks");
        this.obj = GameObject.Instantiate(model);
    }
}
