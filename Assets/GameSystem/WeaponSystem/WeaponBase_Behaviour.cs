using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase_Behaviour : MonoBehaviour
{
    public ParticleSystem Muzzleflash;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void PlayFireSound()
    {
        
    }

    public virtual void EmptyFire()
    {
        
    }

    public virtual void CreateFireParticle()
    {
        Muzzleflash.Play();
    }
}
