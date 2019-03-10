using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotate_coefficent;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotate_coefficent * Time.deltaTime));
    }
}
