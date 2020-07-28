using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_M870 : WeaponBase
{
    public Weapon_M870()
    {
        WeaponName = "M870";
        Interval = 0.3f;
        Continuous = true;
        Capacity = 10;
    }
}
