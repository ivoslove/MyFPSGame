using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_MP5 : WeaponBase
{
    public Weapon_MP5()
    {
        WeaponName = "MP5K";
        Interval = 0.3f;
        Continuous = true;
        Capacity = 10;
    }
}

