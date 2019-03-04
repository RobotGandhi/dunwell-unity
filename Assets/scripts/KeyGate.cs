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
        // Get and enable animator
        anim_controller = GetComponent<Animator>();
        anim_controller.enabled = true;

        unlocked = false;
    }

    public void Open()
    {
        unlocked = true;
        anim_controller.SetTrigger("open");
        
    }

    public bool IsOpen()
    {
        return unlocked;
    }

}
