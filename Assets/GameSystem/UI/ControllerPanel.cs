using System.Collections;
using System.Collections.Generic;
using App.Dispatch;
using UnityEngine;
using UnityEngine.UI;

public class ControllerPanel : MonoBehaviour
{
    [SerializeField]
    private Button _fireButton;
    [SerializeField]
    private Button _jumpButton;
    [SerializeField]
    private Button _reloadButton;

    void Start()
    {
        _fireButton.onClick.AddListener(() => { Dispatcher.DoWork("MobileFire"); });
        _reloadButton.onClick.AddListener(() => { Dispatcher.DoWork("MobileReload"); });
        _jumpButton.onClick.AddListener(() =>
        {

            Dispatcher.DoWork("MobileJump");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
