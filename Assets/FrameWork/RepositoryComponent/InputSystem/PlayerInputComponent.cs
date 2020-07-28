using System.Collections;
using System.Collections.Generic;
using App.Component;
using App.Dispatch;
using UnityEngine;

public class PlayerInputComponent : BaseComponent
{
    private float inputRotHorizontalValue = 0;
    private float inputRotVerticalValue = 0;

    private float inputMoveHorizontalValue = 0;
    private float inputMoveVerticalValue = 0;


    public void RegisterEvent()
    {
        Dispatcher.Listener<float>("SetRot_X",SetRotX);
        Dispatcher.Listener<float>("SetRot_Y", SetRotY);
        Dispatcher.Listener<float>("SetMove_X", SetMoveX);
        Dispatcher.Listener<float>("SetMove_Y", SetMoveY);
    }

    private void SetMoveY(float y)
    {
        inputMoveVerticalValue = y;
    }

    private void SetMoveX(float x)
    {
        inputMoveHorizontalValue = x;
    }

    private void SetRotY(float y)
    {
        inputRotVerticalValue = y;
    }

    private void SetRotX(float x)
    {
        inputRotHorizontalValue = x;
    }

    public float GetRotHorizontalValue()
    {
       
#if UNITY_EDITOR
        inputRotHorizontalValue = Input.GetAxis("Mouse X");
#endif


        return inputRotHorizontalValue;
    }

    public float GetRotVerticalValue()
    {

#if UNITY_EDITOR||UNITY_STANDALONE
        inputRotVerticalValue = Input.GetAxis("Mouse Y");
#endif

        return inputRotVerticalValue;
    }

    public float GetMoveHorizontalValue()
    {

#if UNITY_EDITOR||UNITY_STANDALONE
        inputMoveHorizontalValue = Input.GetAxis("Horizontal");
#endif

        return inputMoveHorizontalValue;

    }

    public bool GetRunningInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetKey(KeyCode.LeftShift);
#endif
        return Input.GetKey(KeyCode.LeftShift);
    }

    public float GetMoveVerticalValue()
    {

#if UNITY_EDITOR||UNITY_STANDALONE
        inputMoveVerticalValue = Input.GetAxis("Vertical");
        //Debug.Log("竖直方向"+ inputMoveVerticalValue.ToString("0.000"));
#endif


        return inputMoveVerticalValue;
    }

    public bool GetSingleFireInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            return true;

#endif
        return false;
    }

    private bool _startContinous;
    private float _interval;


    private float time;
    public bool GetContinousFireInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE

        if (Input.GetMouseButtonDown(0))
        {
            _startContinous = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            time = 0;
            _startContinous = false;
        }
        if (_startContinous)
        {
            if (time < _interval)
            {
                time += Time.deltaTime;
                return false;
            }
            else
            {
                time = 0;
                return true;
            }
        }

#endif
        return false;
    }

    public float GetInterval()
    {
        return _interval;
    }
    public void SetInterval(float interval)
    {
        _interval = interval;
    }

    public bool GetJumpInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

#endif

        return false;
    }

    public bool GetReloadValue()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.R))
        {
            return true;
        }


#endif
        return false;
    }

}
