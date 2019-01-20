using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for everyone that wants to listen to touch events */

public class touch_listener : MonoBehaviour
{
    public virtual void HorizontalSwipe(int direction) { }
    public virtual void VerticalSwipe(int direction) { }
    public virtual void FingerUp() { }
    public virtual void DoubleTap() { }
}
