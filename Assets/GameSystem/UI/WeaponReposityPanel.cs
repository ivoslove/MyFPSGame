using System.Collections;
using System.Collections.Generic;
using App.Dispatch;
using UnityEngine;
using UnityEngine.UI;

public class WeaponReposityPanel : MonoBehaviour
{
    [SerializeField]
    private Toggle _glockToggle;
    [SerializeField]
    private Toggle _akmToggle;
    void Start()
    {
        _glockToggle.onValueChanged.AddListener(OnToggleChangeGlock);
        _akmToggle.onValueChanged.AddListener(OnToggleChangeAKM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnToggleChangeGlock(bool isOn )
    {

        if (isOn)
        {
            Dispatcher.DoWork("ChangeWeapon","Glock");
        }
    }
    public void OnToggleChangeAKM(bool isOn)
    {
        Debug.Log("换akm");
        if (isOn )
        {
            Dispatcher.DoWork("ChangeWeapon", "AKM");
        }
    }
}
