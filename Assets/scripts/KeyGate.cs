using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour
{

    private Animator anim_controller;
    private bool unlocked;
    public string door_color;

    private void Start()
    {
        anim_controller = GetComponent<Animator>();
        unlocked = false;
    }

    public void Open()
    {
        unlocked = true;
        anim_controller.SetTrigger("unlock");
    }

    public bool IsOpen()
    {
        return unlocked;
    }

}
