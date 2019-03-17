using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{

    public Sprite down_sprite;
    [System.NonSerialized]
    public bool toggled = false;

    public void Enable()
    {
        if (!toggled)
        {
            toggled = true;

            // Change sprite!
            GetComponent<SpriteRenderer>().sprite = down_sprite;
            // SFX
            FindObjectOfType<SoundEffects>().PlaySFX("pp1");
        }
    }

}
