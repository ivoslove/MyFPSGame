using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponBase
{
}

public abstract class WeaponBase : IWeaponBase
{
    public string WeaponName;
    //弹夹容量
    public int Capacity;
    //攻击力
    protected int ATK;
    //弹道距离
    public float Range;
    //后坐力
    public float Recoil;
    //是否连发
    protected bool Continuous;
    //是否有瞄准器
    public bool HaveGunSight;
    //连发时间间隔
    protected float Interval;

    //model
    public GameObject weaponGameObject;
    //子弹
    public GameObject BulletGameObject;
    //枪口点
    public GameObject MuzzlePoint;
    //弹夹点
    public GameObject CaseSpawnPoint;


    //弹壳
    public GameObject EmptyCase;
    //微瞄准位置
    public Vector3 AimLocalPos;

    public Vector3 OriginlocalPos;

    public virtual WeaponState  InitWeaponState()
    {
        WeaponState state=new WeaponState();
        state.WeaponName = WeaponName;
        state.Capacity = Capacity;
        state.ATK = ATK;
        state.Range = Range;
        state.Recoil = Recoil;
        state.Interval = Interval;
        state.LeftBullit = Capacity;
        state.Continuous = Continuous;
        return state;
    }

}
