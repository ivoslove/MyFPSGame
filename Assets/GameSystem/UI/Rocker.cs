using System.Collections;
using System.Collections.Generic;
using App.Dispatch;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 绑定到摇杆上的摇杆类,参考半径50
/// </summary>
public class Rocker : MonoBehaviour
{

    public RockerTrigger Trigger;
    Vector2 m_offet;//偏移向量
    Vector2 m_originalPos;//摇杆原始屏幕坐标
    Touch[] touches;//屏幕上触控点数组
    int touch_Id = -1;//触控点数组下标
    bool isMove = false;//是否移动
    float m_ScreenScale;


    public int MoveTouchId => touch_Id;

    public int FingerId
    {
        get
        {
            if (touch_Id != -1)
            {
                return touches[touch_Id].fingerId;
            }

            return -1;
        }
    }

    /// <summary>
    /// 给外部调用的偏移向量,告知摇杆参数
    /// </summary>
    public Vector3 Offet
    {
        get
        {
            return m_offet;
        }
    }

    // Use this for initialization
    void Start()
    {

        m_originalPos = transform.position;//摇杆中心的屏幕坐标位置
        m_ScreenScale = Screen.width / 1080f;

    }

    // Update is called once per frame
    void Update()
    {
      
        if(!Trigger.IsTrigger)
            return;
        //得到屏幕触控数组
        touches = Input.touches;


        if (touches.Length > 0)//如果触点开启
        {
            //得到离摇杆中心最近的触点下标 touch_Id;
            if (touches.Length == 1)//只有一个触点时
            {
                touch_Id = 0;
            }
            else if (touches.Length > 1)//触点大于1个时
            {
                touch_Id = 0;//先假设下标为0
                for (int i = 1; i < touches.Length; i++)//遍历触点数组
                {
                    float dis = Vector2.SqrMagnitude(touches[i].position - m_originalPos);
                    if (Vector2.SqrMagnitude(touches[i].position - m_originalPos) < Vector2.SqrMagnitude(touches[touch_Id].position - m_originalPos))//第i个点比假设的点近
                    {
                        touch_Id = i;//假设的点改为第i个点
                    }
                }

            }

            //如果得到的触点不是取消或抬起
            if (Input.GetTouch(touch_Id).phase != TouchPhase.Canceled && Input.GetTouch(touch_Id).phase != TouchPhase.Ended)
            {

                //触点在摇杆范围内
                if (Vector2.SqrMagnitude(touches[touch_Id].position - m_originalPos) <= 50 * 50 * m_ScreenScale * m_ScreenScale)//50为背景半径
                {
                    isMove = true;//开启遥控
                                  //摇杆开始控制,计算偏移量
                    SetOffetIn();
                }
                else if (isMove)//触点在摇杆范围外,但是遥控已经开启
                {
                    SetOffetOut();
                }
            }
            else// 手指抬起,摇杆回归原始位置
            {
                transform.position = m_originalPos;
                m_offet = Vector2.zero;
                Dispatcher.DoWork<float>("SetMove_X",0);
                Dispatcher.DoWork<float>("SetMove_Y", 0);
                isMove = false;
                touch_Id = -1;
                Trigger.IsTrigger = false;
            }
        }
        else
        {
            transform.position = m_originalPos;
            m_offet = Vector2.zero;
            Dispatcher.DoWork<float>("SetMove_X", 0);
            Dispatcher.DoWork<float>("SetMove_Y", 0);
            isMove = false;
            touch_Id = -1;
        }

    }
    /// <summary>
    /// 触点在操作盘内时
    /// 摇杆控制方法
    /// </summary>
    void SetOffetIn()
    {
        //距离过小视为不偏移摇杆位置不变
        if (Vector2.SqrMagnitude(touches[touch_Id].position - m_originalPos) < 5 * m_ScreenScale)
        {
            GetComponent<Image>().rectTransform.position = m_originalPos;//摇杆定位在原始位置
            m_offet = Vector3.zero;
            Dispatcher.DoWork<float>("SetMove_X", 0);
            Dispatcher.DoWork<float>("SetMove_Y", 0);
        }
        else
        {
            //摇杆位置追踪
            GetComponent<Image>().rectTransform.position = touches[touch_Id].position;
            m_offet = touches[touch_Id].position - m_originalPos;//赋值偏移值
            m_offet = m_offet.normalized;//归一化
            Dispatcher.DoWork<float>("SetMove_X", m_offet.x);
            Dispatcher.DoWork<float>("SetMove_Y", m_offet.y);
        }
    }
    /// <summary>
    /// 触点在操作盘外时
    /// 摇杆控制方法
    /// </summary>
    void SetOffetOut()
    {
        Vector2 tempDir;//临时偏移向量
        tempDir = touches[touch_Id].position - m_originalPos;
        //更新摇杆位置:距离原始位置127各单位
        GetComponent<Image>().rectTransform.position = m_originalPos + (tempDir.normalized) * 81 * m_ScreenScale;
        //偏移量
        m_offet = tempDir.normalized;//归一化
        Dispatcher.DoWork<float>("SetMove_X", m_offet.x);
        Dispatcher.DoWork<float>("SetMove_Y", m_offet.y);

    }
    private void OnGUI()
    {
        //GUIStyle style = new GUIStyle(); //实例化一个新的GUIStyle，名称为style ，后期使用
        //style.fontSize = 50; //字体的大小设置数值越大，字越大，默认颜色为黑色 
        //style.normal.textColor = new Color(1, 1, 1); //设置文本的颜色为 新的颜色(0,0,0)修改值-代表不同的颜色,值为整数 我个人觉得有点像RGB的感觉
        //if(touch_Id!=-1)
        //     GUI.Label(new Rect(20, 30, 300, 60), "deltaPos:" +touches[touch_Id].deltaPosition.ToString(), style);
        //GUI.Label(new Rect(20, 100, 300, 60), "是否触发:" + Trigger.IsTrigger, style);
        //GUI.Label(new Rect(20, 30, 300, 60), "原始位置:" + m_originalPos.ToString(), style);
        //GUI.Label(new Rect(20, 100, 300, 60), "摇杆位置:" + GetComponent<Image>().rectTransform.position.ToString(), style);
        //if (Input.touches.Length != 0)
        //    GUI.Label(new Rect(20, 170, 300, 60), "触点位置:" + touches[touch_Id].position.ToString(), style);
        //GUI.Label(new Rect(20, 240, 300, 60), "屏幕分辨率:" + Screen.currentResolution, style);
    }
}