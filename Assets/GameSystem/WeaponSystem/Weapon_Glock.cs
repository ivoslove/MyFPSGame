using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TaskExtension;
using Random = UnityEngine.Random;

public class Weapon_Glock : WeaponBase
{
    public Weapon_Glock()
    {
        WeaponName = "Glock";
        Interval = 0.3f;
        Continuous = false;
        Capacity = 10;
        Range = 100f;
        HaveGunSight = true;
        AimLocalPos=new Vector3(0,-2.18f,0.05f);
        OriginlocalPos=new Vector3(0.45f, - 2.35f, 0.05f);
    }


}
