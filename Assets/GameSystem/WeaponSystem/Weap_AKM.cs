using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AKM : WeaponBase
{
    public Weapon_AKM()
    {
        WeaponName = "AKM";
        Interval = 0.1f;
        Continuous = true;
        Capacity = 30;
        Recoil = 0.2f;
        Range = 150f;
        HaveGunSight = true;
        OriginlocalPos = new Vector3(0.53f, -2.29f, -0.32f);
        AimLocalPos=new Vector3(0,-2.07f,-0.72f);
    }
}
