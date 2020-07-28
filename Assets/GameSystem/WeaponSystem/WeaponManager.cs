using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Component;
using App.Dispatch;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private  Dictionary<string,WeaponBase> _weaponRepository=new Dictionary<string, WeaponBase>();

    private Weapon_AKM akm;
    private Weapon_M870 m870;
    private Weapon_MP5 mp5k;
    private Weapon_Glock glock;

    private WeaponBase _currentWeapon;

    private Animator _weaponAnimator;

    private Transform weaponHandNode;
    private Dictionary<string, Animator> _weaponAnimatorStateRepository = new Dictionary<string, Animator>();

    private WeaponState _weaponState;

    private Animator _animatorState;
    public WeaponState WeaponState
    {
        get => _weaponState;
    }
    [SerializeField]
    private Transform _shortPoint;

    public float WeaponRecoil
    {
        get
        {
            if (_currentWeapon == null)
                return 0;
            return _currentWeapon.Recoil;
        }
    }
    /// <summary>
    /// 后坐力参数
    /// </summary>
    private float _currenReoil=0;
    /// <summary>
    /// 后坐力阻尼系数
    /// </summary>
    private float _reCoilDrag=3f;

    public float CurrenReoil
    {
        get => _currenReoil;
        set => _currenReoil = value;
    }

    public AudioManager _audioManager;

    public Animator WeaponAnimator
    {
        get
        {
            if (_currentWeapon==null)
            {
                return null;
            }

            return _weaponAnimatorStateRepository[_currentWeapon.WeaponName];
        }
    }

    public WeaponBase CurrentWeapon
    {
        get => _currentWeapon;
        set => _currentWeapon = value;
    }

    public void InitWeaponRepository()
    {

        weaponHandNode = transform;
        akm = new Weapon_AKM();
        InstantiateWeapon(akm);
        _weaponRepository.Add("AKM", akm);
        _weaponAnimatorStateRepository.Add("AKM", weaponHandNode.Find("AKM").GetComponent<Animator>());
        m870 = new Weapon_M870();
        InstantiateWeapon(m870);
        _weaponRepository.Add("M870", m870);
        _weaponAnimatorStateRepository.Add("M870", weaponHandNode.Find("M870").GetComponent<Animator>());
        mp5k = new Weapon_MP5();
        InstantiateWeapon(mp5k);
        _weaponRepository.Add("MP5K", mp5k);
        _weaponAnimatorStateRepository.Add("MP5K", weaponHandNode.Find("MP5K").GetComponent<Animator>());
        glock = new Weapon_Glock();
        InstantiateWeapon(glock);
        _weaponRepository.Add("Glock", glock);

        _weaponAnimatorStateRepository.Add("Glock", weaponHandNode.Find("Glock").GetComponent<Animator>());

    }

    public void SetDefaultWeapon()
    {
        _currentWeapon = Get<Weapon_Glock>();
        InstantiateWeapon(_currentWeapon);
        _weaponState= _currentWeapon.InitWeaponState();
        _animatorState = _currentWeapon.weaponGameObject.GetComponent<Animator>();
    }

    public void ChangeWeapon(string weaponName)
    {
        switch (weaponName)
        {
            case "Glock":
                _currentWeapon.weaponGameObject.SetActive(false);
                _currentWeapon = glock;
                _animatorState= _currentWeapon.weaponGameObject.GetComponent<Animator>();
                glock.weaponGameObject.SetActive(true);
                Draw();
                _weaponState = _currentWeapon.InitWeaponState();
                break;
            case "AKM":
                _currentWeapon.weaponGameObject.SetActive(false);
                _currentWeapon = akm;
                _animatorState = _currentWeapon.weaponGameObject.GetComponent<Animator>();
                akm.weaponGameObject.SetActive(true);
                Draw();
                _weaponState = _currentWeapon.InitWeaponState();
                break;
        }

        World.GetComponents<PlayerInputComponent>().FirstOrDefault().SetInterval(_weaponState.Interval);
    }

    private void PlayFireAnimation()
    {
        _animatorState.Play("FPSHand|Fire");
    }

    private void InstantiateWeapon(WeaponBase weapon)
    {
        weapon.weaponGameObject = transform.Find(weapon.WeaponName).gameObject;
        weapon.MuzzlePoint = weapon.weaponGameObject.transform.Find("MuzzlePoint").gameObject;
        weapon.CaseSpawnPoint = weapon.weaponGameObject.transform.Find("CaseSpawn").gameObject;
        
    }

    public void Reload()
    {
        _animatorState.Play("FPSHand|Reload");
    }

    public void Draw()
    {
        _animatorState.Play("FPSHand|Draw");
    }

    public T Get<T>() where T : WeaponBase
    {
        return _weaponRepository.Values.ToList().FirstOrDefault(p => typeof(T) == p.GetType()) as T;
    }

    public void Fire()
    {
        if (_weaponState.LeftBullit > 0)
        {
            //开火给后坐力
            _currenReoil = _currentWeapon.Recoil;
            PlayFireAnimation();
            PlayFireSound();
            _currentWeapon.weaponGameObject.GetComponent<WeaponBase_Behaviour>().CreateFireParticle();
            _weaponState.LeftBullit--;
            Debug.Log("剩余子弹:"+_weaponState.LeftBullit);
            RaycastHit hit;
            Vector3 originPoint = _shortPoint.position;
            Vector3 dir = _shortPoint.forward;
            float range = _currentWeapon.Range;
            if (Physics.Raycast(originPoint, dir, out hit, range))
            {
                
                Debug.DrawLine(originPoint, hit.point, Color.green, 150.0f);

                FireHitParticle hitSpark= ObjectPoolFactory<FireHitParticle>.GetObject();
           
                hitSpark.obj.transform.position = hit.point;
                hitSpark.obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                hitSpark.obj.SetActive(true);
                vp_Timer.In(1f, () =>
                {
                    ObjectPoolFactory<FireHitParticle>.Collect(hitSpark);
                    hitSpark.obj.SetActive(false);
                });
                BulletImpact bulletImpact = ObjectPoolFactory<BulletImpact>.GetObject();
                bulletImpact.obj.SetActive(true);
                bulletImpact.obj.transform.position = hit.point;
                bulletImpact.obj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                vp_Timer.In(5f, () =>
                {
                    ObjectPoolFactory<BulletImpact>.Collect(bulletImpact);
                    bulletImpact.obj.SetActive(false);
                });
            }

        }
        else
        {
            Debug.LogError("没有子弹了");
            _currentWeapon.weaponGameObject.GetComponent<WeaponBase_Behaviour>().EmptyFire();
        }
    }

    private void PlayFireSound()
    {
        _currentWeapon.weaponGameObject.GetComponent<WeaponBase_Behaviour>().PlayFireSound();
    }


    #region Mono-life

    void Awake()
    {
        Dispatcher.Listener<string>("ChangeWeapon",ChangeWeapon);
    }

    void Start()
    {

    }

    void Update()
    {
        if (_currenReoil <= 0)
        {
            _currenReoil = 0;
        }
        else
        {
            _currenReoil -= _currentWeapon.Recoil * Time.deltaTime* _reCoilDrag;
        }
    }
    #endregion
}

public class WeaponState
{
    //武器名称
    public string WeaponName;
    //弹夹容量
    public int Capacity;
    //攻击力
    public int ATK;
    //弹道距离
    public float Range;
    //后坐力
    public float Recoil;
    //是否连发
    public bool Continuous;
    //是否有瞄准器
    public bool HaveGunSight;
    //连发时间间隔
    public float Interval;
    //剩余弹量
    public int LeftBullit;

}
