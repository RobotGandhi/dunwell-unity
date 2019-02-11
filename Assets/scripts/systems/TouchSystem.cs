using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSystem : MonoBehaviour
{
    const float swipeThreshshold = 50;
    private bool canSwipe = true;
    private List<touch_listener> touchListenerList = new List<touch_listener>();

    bool listenForDoubleTap = false;

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began && listenForDoubleTap)
            {
                foreach (touch_listener x in touchListenerList)
                {
                    x.DoubleTap();
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (canSwipe)
                {
                    if (Mathf.Abs(touch.deltaPosition.x) > swipeThreshshold)
                    {
                        foreach(touch_listener x in touchListenerList) {
                            x.HorizontalSwipe((int)Mathf.Clamp(touch.deltaPosition.x, -1, 1));
                        }
                        canSwipe = false;
                    }
                    else if (Mathf.Abs(touch.deltaPosition.y) > swipeThreshshold)
                    {
                        foreach (touch_listener x in touchListenerList) {
                            x.VerticalSwipe((int)Mathf.Clamp(touch.deltaPosition.y, -1, 1));
                        }
                        canSwipe = false;
                    }
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                foreach(touch_listener x in touchListenerList) {
                    x.FingerUp();
                }
                if(canSwipe)
                    StartCoroutine("DoubleTap");
            }
        }
        else
        {
            if(!canSwipe)
                canSwipe = true;
        }
    }

    public void AddTouchListener(touch_listener _obj)
    {
        touchListenerList.Add(_obj);
    }

    IEnumerator DoubleTap()
    {
        listenForDoubleTap = true;
        yield return new WaitForSeconds(2.0f);
        listenForDoubleTap = false;
    }
}
