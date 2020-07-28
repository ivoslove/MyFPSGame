using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Component;
using App.Dispatch;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressChecker : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    // Start is called before the first frame update
    private bool press;
    private float time;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (press)
        {
          var input=  World.GetComponents<PlayerInputComponent>().FirstOrDefault();
          float interval = input.GetInterval();

          if (time < interval)
          {
              time += Time.deltaTime;
          }
          else
          {
              time = 0;
             Dispatcher.DoWork("MobileFire");
          }
        }
        else
        {
            time = 0;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");
        press = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");
        press = false;
    }
}
