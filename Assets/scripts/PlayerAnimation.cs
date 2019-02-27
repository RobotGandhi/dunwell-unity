﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Player player;
    Animator anim_controller;

    private void Start()
    {
        // Get main player class component
        player = GetComponent<Player>();
        player.player_animation = this;
        // Get the animator component
        anim_controller = GetComponent<Animator>();
        anim_controller.SetBool("idle", true);
        anim_controller.SetBool("holding_item", false);
        anim_controller.SetBool("moving", false);
    }

    public void StartMoving(Enums.PlayerMoveDirection direction)
    {
        anim_controller.SetTrigger("move_trigger");
        anim_controller.SetBool("moving", true);
        anim_controller.SetBool("idle", false);
        anim_controller.SetInteger("move_direction", (int)direction);
    }

    public void StartIdle()
    {
        anim_controller.SetBool("moving", false);
        anim_controller.SetBool("idle", true);
    }

    public void SetMoveDirection(Enums.PlayerMoveDirection direction)
    {
        anim_controller.SetInteger("move_direction", (int)direction);
    }

    public void Die()
    {
        anim_controller.SetTrigger("die");
    }

    public void ItemChange()
    {
        if(player.current_item == null)
        {
            anim_controller.SetBool("holding_item", false);
        }
        else
        {
            anim_controller.SetBool("holding_item", true);
        }
    }

    public void PerformCombat(Enums.CombatResult combat_result) 
    {
        // Animation
        if (combat_result != Enums.CombatResult.PLAYER_DIED)
        {
            if (player.current_item != null)
            {
                if (player.current_item.GetComponent<Item>().item_type == Item.ItemType.WEAPON)
                {
                    player.StartCoroutine("HideSwordForAnimation");
                    anim_controller.SetTrigger("attack_sword");
                }
                else if (player.current_item.GetComponent<Item>().item_type == Item.ItemType.HEALTH)
                {
                    anim_controller.SetTrigger("attack_item");
                }
                else
                {
                    anim_controller.SetTrigger("attack");
                }
            }
            else
            {
                anim_controller.SetTrigger("attack");
            }
        }
    }

}
