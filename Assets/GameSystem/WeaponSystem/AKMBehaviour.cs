using System.Collections;
using System.Collections.Generic;
using App.Dispatch;
using UnityEngine;

public class AKMBehaviour : WeaponBase_Behaviour
{
    public WeaponManager _weaponManager;
    private float _bulletEjectingSpeed = 5.0f;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip magOutSound;
    [SerializeField] private AudioClip boltSound;
    [SerializeField] private AudioClip magInSound;
    [SerializeField] private AudioClip _emptyAudioClip;
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCaseOut()
    {
        Weapon_AKM Glock = _weaponManager.Get<Weapon_AKM>();
        AKMEmptyCase emptyCase = ObjectPoolFactory<AKMEmptyCase>.GetObject();
        var ejectedCase = emptyCase.obj;
        ejectedCase.SetActive(true);
        ejectedCase.transform.position = Glock.CaseSpawnPoint.transform.position;
        ejectedCase.transform.rotation = Glock.CaseSpawnPoint.transform.rotation;
        var caseRigidbody = ejectedCase.GetComponent<Rigidbody>();
        caseRigidbody.velocity =
            Glock.CaseSpawnPoint.transform.TransformDirection(-Vector3.left * _bulletEjectingSpeed);
        caseRigidbody.AddTorque(Random.Range(-0.2f, 0.2f), Random.Range(0.1f, 0.2f), Random.Range(-0.2f, 0.2f));
        caseRigidbody.AddForce(0, Random.Range(2.0f, 4.0f), 0, ForceMode.Impulse);
        vp_Timer.In(10f, () =>
        {
            ejectedCase.SetActive(false);
            ObjectPoolFactory<AKMEmptyCase>.Collect(emptyCase);
        });
    }

    void OnReloadEnd()
    {
        _weaponManager.WeaponState.LeftBullit = _weaponManager.CurrentWeapon.Capacity;
        Dispatcher.DoWork("SetReloadingState", false);
    }


    void OnMagOut()
    {
        Dispatcher.DoWork("SetReloadingState", true);
        _weaponManager._audioManager.Play(magOutSound);
    }



    void OnDraw()
    {
        _weaponManager._audioManager.Play(boltSound);
    }


    void OnMagIn()
    {
        _weaponManager._audioManager.Play(magInSound);
    }

    void OnBoltPulled()
    {
        _weaponManager._audioManager.Play(boltSound);
    }

   public override void PlayFireSound()
    {
        _weaponManager._audioManager.Play(fireSound);
        Debug.Log("播放声音");
    }

   public override void EmptyFire()
   {
  
       _weaponManager._audioManager.Play(_emptyAudioClip);
   }
}
