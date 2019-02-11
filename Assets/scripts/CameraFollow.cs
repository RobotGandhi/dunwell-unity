﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float follow_speed = 1.0f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, follow_speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
