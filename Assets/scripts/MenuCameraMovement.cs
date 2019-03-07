using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCameraMovement : TouchListener
{
    public Camera mainCamera;
    public Scrollbar slider;
    public GameObject listOfCameraPoints;

    bool moving = false;
    bool topOfLevel = true;
    bool inWorld = false;
    int currentWorld = 0;
    static int maxWorlds = 4;

    TouchSystem touchSystem;
    Vector3 newPosition;
    List<RectTransform> levelCameraPoints = new List<RectTransform>();
    List<RectTransform> topOfLevelCameraPoints = new List<RectTransform>();
    List<RectTransform> worldCameraPoints = new List<RectTransform>();
    List<Scrollbar> scrollbars = new List<Scrollbar>();

    void Awake()
    {
        touchSystem = FindObjectOfType<TouchSystem>();
        touchSystem.AddTouchListener(this);
        newPosition = mainCamera.transform.position;
        foreach (RectTransform transform in listOfCameraPoints.GetComponentInChildren<RectTransform>())
        {
            if (transform.tag == "Level")
                levelCameraPoints.Add(transform);
            else if (transform.tag == "Top Of Level")
                topOfLevelCameraPoints.Add(transform);
            else if (transform.tag == "World")
                worldCameraPoints.Add(transform);
        }
        foreach (Scrollbar bar in FindObjectsOfType<Scrollbar>())
        {
            scrollbars.Add(bar);
        }

        levelCameraPoints.Sort(delegate(RectTransform r1, RectTransform r2) 
        {
            return r1.gameObject.name.CompareTo(r2.gameObject.name);
        });
        topOfLevelCameraPoints.Sort(delegate (RectTransform r1, RectTransform r2)
        {
            return r1.gameObject.name.CompareTo(r2.gameObject.name);
        });
        worldCameraPoints.Sort(delegate (RectTransform r1, RectTransform r2)
        {
            return r1.gameObject.name.CompareTo(r2.gameObject.name);
        });


        scrollbars.Sort(delegate(Scrollbar s1, Scrollbar s2)
        {
            return s1.transform.parent.transform.parent.name.CompareTo(s2.transform.parent.transform.parent.name);
        });
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

        if (scrollbars[currentWorld].value > 0.99 && !topOfLevel && !inWorld)
        {
            moving = true;
            topOfLevel = true;
            newPosition = topOfLevelCameraPoints[currentWorld].position;
        }
        else if (scrollbars[currentWorld].value < 0.99 && topOfLevel)
        {
            moving = true;
            topOfLevel = false;
            newPosition = levelCameraPoints[currentWorld].position;
        }
    }

    public override void VerticalSwipe(int direction)
    {
        if (topOfLevel && direction == -1)
        {
            scrollbars[currentWorld].value = 1;
            newPosition = worldCameraPoints[currentWorld].position;
            moving = true;
            topOfLevel = false;
            inWorld = true;
        }
        else if (!moving && inWorld && direction == 1)
        {
            newPosition = topOfLevelCameraPoints[currentWorld].position;
            moving = true;
            topOfLevel = true;
            inWorld = false;
        }
        Debug.Log(direction + "Vertical");
    }

    public override void HorizontalSwipe(int direction)
    {
        if (inWorld && currentWorld - direction >= 0 && currentWorld - direction < maxWorlds)
        {
            currentWorld -= direction;
            newPosition = worldCameraPoints[currentWorld].position;
            moving = true;
        }
    }

    public void MoveToWorld()
    {
        scrollbars[currentWorld].value = 1;
        newPosition = worldCameraPoints[currentWorld].position;
        inWorld = true;
        moving = true;
    }
}  