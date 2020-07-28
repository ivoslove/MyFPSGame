using System.Collections;
using System.Collections.Generic;
using App.Dispatch;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchRotate : MonoBehaviour
{
    private Vector2 _offset;
    private int _touchId;

    private Touch[] touches;

    public Rocker moveRocker;

    private bool clickUI;


    void Start()
    {
    }


    void Update()
    {
        touches = Input.touches;

        if (touches.Length > 0)
        {

            if (touches.Length == 1)
            {
                //不是移动的touch
                if (!clickUI && touches[0].fingerId != moveRocker.FingerId)
                {
                    var touch = touches[0];
                    _touchId = touch.fingerId;
                    if (touch.phase == TouchPhase.Moved)
                    {
                        _offset = touch.deltaPosition;
                        Dispatcher.DoWork("SetRot_X", _offset.x);
                        Dispatcher.DoWork("SetRot_Y", _offset.y);
                    }
                    else
                    {
                        _offset = Vector2.zero;
                        Dispatcher.DoWork("SetRot_X", 0f);
                        Dispatcher.DoWork("SetRot_Y", 0f);
                    }
                }
                else
                {
                    _touchId = -1;
                    _offset = Vector2.zero;
                    Dispatcher.DoWork("SetRot_X", 0f);
                    Dispatcher.DoWork("SetRot_Y", 0f);
                }
            }
            else //多个touch
            {
                Debug.LogError("多个touch");
                int touchId = -1;
                //排除移动的touch
                for (int i = 0; i < touches.Length; i++)
                {
                    var touch = touches[i];
                    if (touch.fingerId != moveRocker.FingerId)
                    {
                        touchId = touch.fingerId;
                    }
                }

                if (touchId == -1)
                {
                    Debug.LogError("竟然没找到旋转的touch");
                    _touchId = -1;
                    _offset = Vector2.zero;
                    Dispatcher.DoWork("SetRot_X", 0f);
                    Dispatcher.DoWork("SetRot_Y", 0f);
                }
                else
                {
                    if (Input.GetTouch(touchId).phase == TouchPhase.Moved)
                    {
                        Debug.LogError("开始旋转");
                        _offset = Input.GetTouch(touchId).deltaPosition;
                        Dispatcher.DoWork("SetRot_X", _offset.x);
                        Dispatcher.DoWork("SetRot_Y", _offset.y);
                    }
                    else
                    {
                        Debug.LogError("找到touch，但是没有旋转");
                        _offset = Vector2.zero;
                        Dispatcher.DoWork("SetRot_X", 0f);
                        Dispatcher.DoWork("SetRot_Y", 0f);
                    }
                }
            }

        }
        else
        {
            _touchId = -1;
            _offset = Vector2.zero;
            Dispatcher.DoWork("SetRot_X", 0f);
            Dispatcher.DoWork("SetRot_Y", 0f);
        }
    }

    private void OnGUI()
    {
        //GUIStyle style = new GUIStyle(); //实例化一个新的GUIStyle，名称为style ，后期使用
        //style.fontSize = 50; //字体的大小设置数值越大，字越大，默认颜色为黑色 
        //style.normal.textColor = new Color(1, 1, 1); //设置文本的颜色为 新的颜色(0,0,0)修改值-代表不同的颜色,值为整数 我个人觉得有点像RGB的感觉

        //GUI.Label(new Rect(500, 30, 300, 60), "offset:" + _offset.ToString(), style);

    }
}
