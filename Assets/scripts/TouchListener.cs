using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for everyone that wants to listen to touch events */

public class TouchListener : MonoBehaviour
{
    public virtual void HorizontalSwipe(int direction) { }
    public virtual void VerticalSwipe(int direction) { }
    public virtual void FingerUp() { }
    public virtual void DoubleTap() { }
    public virtual void FingerDown(int index, Vector2 pos) { }
}
