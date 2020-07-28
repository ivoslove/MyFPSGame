using System.Collections;
using System.Collections.Generic;
using App.Dispatch;
using UnityEngine;

public class MP5Behaviour : MonoBehaviour
{
    public WeaponManager _weaponManager;
    private float bulletEjectingSpeed = 5.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCaseOut()
    {
        Weapon_MP5 Glock = _weaponManager.Get<Weapon_MP5>();


        MP5EmptyCase emptyCase = ObjectPoolFactory<MP5EmptyCase>.GetObject();
        var ejectedCase = emptyCase.obj;
        ejectedCase.SetActive(true);
        ejectedCase.transform.position = Glock.CaseSpawnPoint.transform.position;
        ejectedCase.transform.rotation = Glock.CaseSpawnPoint.transform.rotation;
        var caseRigidbody = ejectedCase.GetComponent<Rigidbody>();
        caseRigidbody.velocity = Glock.CaseSpawnPoint.transform.TransformDirection(-Vector3.left * bulletEjectingSpeed);
        caseRigidbody.AddTorque(Random.Range(-0.2f, 0.2f), Random.Range(0.1f, 0.2f), Random.Range(-0.2f, 0.2f));
        caseRigidbody.AddForce(0, Random.Range(2.0f, 4.0f), 0, ForceMode.Impulse);
        vp_Timer.In(10f, () =>
        {
            ejectedCase.SetActive(false);
            ObjectPoolFactory<MP5EmptyCase>.Collect(emptyCase);
        });
    }

    void OnReloadEnd()
    {
        _weaponManager.WeaponState.Capacity = _weaponManager.CurrentWeapon.Capacity;
        Dispatcher.DoWork("SetReloadingState", false);
    }

    void OnDraw()
    {

    }

    void OnMagOut()
    {
        Dispatcher.DoWork("SetReloadingState", true);
    }

    void OnMagIn()
    {

    }

    void OnBoltPulled()
    {

    }
}
