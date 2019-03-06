using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCameraMovement : TouchListener
{
    public Camera mainCamera;
    public Scrollbar slider;
    TouchSystem touchSystem;
    Vector3 newPosition;
    bool moving = false;
    bool topOfLevel = false;
    bool inWorld = false;

    void Awake()
    {
        touchSystem = FindObjectOfType<TouchSystem>();
        touchSystem.AddTouchListener(this);
    }

    void Update()
    {
        if (moving)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newPosition, 2 * Time.deltaTime);
            if (Mathf.Abs(mainCamera.transform.position.sqrMagnitude - newPosition.sqrMagnitude) < 0.01)
            {
                mainCamera.transform.position = newPosition;
                moving = false;
            }
        }
        else if (slider.value > 0.99 && !topOfLevel)
        {
            moving = true;
            topOfLevel = true;
            newPosition = mainCamera.gameObject.transform.position + new Vector3(0, 3, 0);
        }
        else if (slider.value < 0.99 && topOfLevel)
        {
            moving = true;
            topOfLevel = false;
            newPosition = mainCamera.gameObject.transform.position + new Vector3(0, -3, 0);
        }
    }

    public override void VerticalSwipe(int direction)
    {
        if (!moving)
        {
            newPosition = mainCamera.gameObject.transform.position + new Vector3(0, direction* -2, 0);
            moving = true;
        }
    }
}  