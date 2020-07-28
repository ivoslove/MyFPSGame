using System.Collections;
using System.Collections.Generic;
using App.Component;
using App.Dispatch;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MouseLockComponent _mouseLock;

    private PlayerInputComponent _input;

    private Transform _firstPersonCharacter;


    private WeaponManager _weaponManager;


    private Rigidbody _rigidbody;
    private CharacterController _characterController;


    private bool _iswalking;
    private bool _isrunning;
    private bool _isFiring;
    private bool _isGrounding;
    private bool __isReloading;
    private bool _isChangingWeapon;
    private bool _jump;
    private bool _jumping;
    private bool m_PreviouslyGrounded;
    private bool _isAiming;

    private CollisionFlags _CollisionFlags;

    private Vector3 _moveDir = Vector3.zero;



    [SerializeField] private float _stickToGroundForce;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _gravityMultiplier;


    #region Mono-life

    void Awake()
    {
        _input = World.CreateComponent<PlayerInputComponent>("PlayerInput");
        _input.RegisterEvent();
        _mouseLock = World.CreateComponent<MouseLockComponent>("MouseLock");
        _firstPersonCharacter = transform.Find("FirstPersonCharacter");
        _rigidbody = transform.GetComponent<Rigidbody>();
        _characterController = transform.GetComponent<CharacterController>();
    }
    void Start()
    {
        _weaponManager = GetComponentInChildren<WeaponManager>();
        _weaponManager.InitWeaponRepository();
        _weaponManager.SetDefaultWeapon();
        _weaponManager.Draw();
        _input.SetInterval(_weaponManager.WeaponState.Interval);

        Dispatcher.Listener<bool>("SetReloadingState",SetReloadingState);
        Dispatcher.Listener("MobileFire", MobileFire);
        Dispatcher.Listener("MobileReload", MobileReload);
        Dispatcher.Listener("MobileJump", MobileJump);
    }

    private void MobileFire()
    {
        if (__isReloading || _isChangingWeapon)
            return;
        //单发
        _weaponManager.Fire();

    }

    // Update is called once per frame
    void Update()
    {
        //EventSystem uiEventSystem = FindObjectOfType<EventSystem>();
        //if(uiEventSystem)
        //落地声音
        if (!m_PreviouslyGrounded && _characterController.isGrounded)
        {
            //StartCoroutine(m_JumpBob.DoBobCycle());
            //PlayLandingSound();
            _moveDir.y = 0f;
            _jumping = false;
        }
        if (!_characterController.isGrounded && !_jumping && m_PreviouslyGrounded)
        {
            _moveDir.y = 0f;
        }
        m_PreviouslyGrounded = _characterController.isGrounded;
        RotateView();
        Aiming();
#if UNITY_EDITOR || UNITY_STANDALONE
        EditorFire();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _weaponManager.ChangeWeapon("Glock");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _weaponManager.ChangeWeapon("AKM");
        }
        if (!__isReloading&& _input.GetReloadValue())
        {
            _weaponManager.Reload();
        }
#endif
        //_mouseLock.UpdateCursorLock();
    }

    void FixedUpdate()
    {
        Movement();
    }

    #endregion




    #region Private Method

    void RotateView()
    {
        float random = Random.Range(-1f, 1f) * 1;

        float randomHorizitalRecol = random * _weaponManager.CurrenReoil;
        float yRot = _input.GetRotHorizontalValue() * SystemConfig.RotHorizotalInputSensitivity+ randomHorizitalRecol;
        transform.localRotation *= Quaternion.Euler(0,yRot,0);
        //加上后坐力影响
        float xRot = _input.GetRotVerticalValue() * SystemConfig.RotVerticalInputSensitivity+_weaponManager.CurrenReoil;


        _firstPersonCharacter.localRotation*= Quaternion.Euler(-xRot,0, 0);
        _firstPersonCharacter.localRotation = _mouseLock.ClampRotationAroundXAxis(_firstPersonCharacter.localRotation);
    }
     void Movement()
     {
         float x = _input.GetMoveHorizontalValue() ;
         float y = _input.GetMoveVerticalValue() * SystemConfig.MoveSpeed;
        Vector2 dirDelta= new Vector2(x,y);
        Vector3 desiredMove = transform.forward * dirDelta.y + transform.right * dirDelta.x;
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out hitInfo,
            _characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;


        float speed= SystemConfig.MoveSpeed;
        if (_input.GetRunningInput()&&!_isAiming)
        {
            speed *=1.8f ;
            _iswalking = false;
            _isrunning = true;

        }
        else
        {
            _isrunning = false;
            if (x == 0 && y == 0)
            {
                _iswalking = false;
            }
            else
            {
                _iswalking = true;
            }
        }
        _moveDir.x = desiredMove.x * speed;
        _moveDir.z = desiredMove.z * speed;
 
        if (_characterController.isGrounded)
        {
            _moveDir.y = -_stickToGroundForce;

            if (_jump)
            {
                _moveDir.y =_jumpSpeed;
                PlayJumpSound();
                _jump = false;
                _jumping = true;
            }
        }
        else
        {
            _moveDir += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;
        }


        if (_characterController.isGrounded)
        {
            _rigidbody.drag = 5f;
            if (_input.GetJumpInput())
            {
                _jump = true;
                _rigidbody.drag = 0f;
                _isGrounding = false;
                _rigidbody.drag = 0f;
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
                _rigidbody.AddForce(new Vector3(0f, SystemConfig.JumpForce, 0f), ForceMode.Impulse);
            }
        }
        else
        {
            _rigidbody.drag = 0;
        }
        _CollisionFlags = _characterController.Move(_moveDir * Time.fixedDeltaTime);
    }

     private void MobileJump()
     {
         Debug.Log("jump");
         _jump = true;
         _rigidbody.drag = 0f;
         _isGrounding = false;
         _rigidbody.drag = 0f;
         _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
         _rigidbody.AddForce(new Vector3(0f, SystemConfig.JumpForce, 0f), ForceMode.Impulse);
    }
     private void PlayJumpSound()
     {
     
     }

     void EditorFire()
     {
         if(__isReloading||_isChangingWeapon)
             return;
         //单发
         if (!_weaponManager.WeaponState.Continuous)
         {
             if (_input.GetSingleFireInput())
             {
                 _weaponManager.Fire();
    
             }
               
         }
         else//连发
         {
             if (_input.GetContinousFireInput())
             {
                 _weaponManager.Fire();

             }
            }
     }


     void Aiming()
     {
#if UNITY_EDITOR||UNITY_STANDALONE
        if (_weaponManager.CurrentWeapon.HaveGunSight)
         {
             if (Input.GetMouseButton(1))
             {
                 _isAiming = true;
                 _weaponManager.CurrentWeapon.weaponGameObject.transform.localPosition = Vector3.Lerp(
                     _weaponManager.CurrentWeapon.weaponGameObject.transform.localPosition,
                     _weaponManager.CurrentWeapon.AimLocalPos, 0.7f);
             }
             else
             {
                 _isAiming = false;
             }

             if (!_isAiming)
             {
                 _weaponManager.CurrentWeapon.weaponGameObject.transform.localPosition = Vector3.Lerp(
                     _weaponManager.CurrentWeapon.weaponGameObject.transform.localPosition,
                     _weaponManager.CurrentWeapon.OriginlocalPos, 0.2f);
            }
         }
#endif
    }

     void MobileReload()
     {
         if (!__isReloading)
         _weaponManager.Reload();
    }
    #endregion

    #region MyRegion

    public void SetReloadingState(bool isOn)
    {
        __isReloading = isOn;
    }

    #endregion
}
