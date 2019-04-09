using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Animator anim_controller;
    private SpriteRenderer spre;
    private SoundEffects sfx;
    float open_anim_length;

    private bool unlocked;
    public string door_color;
    public Sprite open_sprite;

    private void Start()
    {
        // Get and enable animator
        anim_controller = GetComponent<Animator>();
        anim_controller.enabled = true;

        // Get the open animation clip
        foreach(AnimationClip clip in anim_controller.runtimeAnimatorController.animationClips)
        {
            if (clip != null)
            {
                open_anim_length = clip.length;
            }
        }

        // Get the sprite renderer
        spre = GetComponent<SpriteRenderer>();
        // Get the sfx player
        sfx = FindObjectOfType<SoundEffects>();

        unlocked = false;
    }

    public void Open()
    {
        StartCoroutine("OpenCoroutine");
    }

    private IEnumerator OpenCoroutine()
    {
        // Play the open sfx
        sfx.PlaySFX("key_unlock");
        sfx.PlaySFX("door_open");
        FindObjectOfType<CameraShake>().DoShake(Constants.LightCamShakeLong);

        // Play the open animation
        anim_controller.enabled = true;
        anim_controller.SetTrigger("open");

        // Wait for the open animation to play 
        yield return new WaitForSeconds(open_anim_length);

        anim_controller.enabled = false;
        spre.sprite = open_sprite;
        spre.sortingOrder -= 2;
        unlocked = true;
    }

    public bool IsOpen() { return unlocked; }

}
