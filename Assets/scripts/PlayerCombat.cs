﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player; // Main player class
    SoundEffects sfx;
    MapManager map_manager;
    GameMaster game_master;
    CameraShake cam_shake;
    SpriteRenderer spre;

    private void Start()
    {
        // Get the main player class component
        player = GetComponent<Player>();
        player.player_combat = this;
        spre = GetComponent<SpriteRenderer>();

        // Get other useful objects around the scene
        sfx = FindObjectOfType<SoundEffects>();
        map_manager = FindObjectOfType<MapManager>();
        game_master = FindObjectOfType<GameMaster>();
        cam_shake = FindObjectOfType<CameraShake>();
    }        

    public void EngageEnemy(Enemy enemy, Vector2 enemy_tile_position)
    {
        Enums.CombatResult result = PerformCombat(enemy);

        if (enemy.GetComponent<DeskGoblin>() != null || enemy.GetComponent<Batty>() != null)
        {
            switch (result)
            {
                case Enums.CombatResult.CLASH:
                    // Play the clash sfx 
                    sfx.PlaySFX("enemy_hurt");
                    cam_shake.DoShake(Constants.LightCamShake);
                    break;
                case Enums.CombatResult.ENEMY_DIED:

                    // Spawn enemy remains?
                    if (enemy.remainsPrefab != null)
                    {
                        GameObject remains_object = Instantiate(enemy.remainsPrefab, enemy.transform.position, Quaternion.identity) as GameObject;
                        remains_object.GetComponent<SpriteRenderer>().sortingOrder = Constants.MapHeight - (int)enemy.tile_position.y-2;
                        remains_object.transform.SetParent(map_manager.map_holder.transform);
                    }

                    // Remove enemy from world
                    Destroy(enemy.gameObject);
                    game_master.current_map.enemy_map.Remove(enemy_tile_position);
                    // Play SFX
                    sfx.PlaySFX("enemy_dead_bones");
                    cam_shake.DoShake(Constants.MediumCamShake);
                    break;
                case Enums.CombatResult.PLAYER_DIED:
                    player.Die();
                    break;
                case Enums.CombatResult.SHIELD_DEFEND:
                    if(enemy.damage != 0)
                        RemoveShield();
                    break;
                default:
                    break;
            }
        }
        else if(enemy.GetComponent<GuardEnemy>() != null)
        {

        }

        // Animation call
        player.player_animation.PerformCombat(result);
    }

    public Enums.CombatResult PerformCombat(Enemy enemy)
    {
        bool hasShield = false;
        bool hasWeapon = false;
        // Does the player have a shield?
        if (player.current_item != null)
        {
            hasShield = (player.current_item.GetComponent<Item>().item_type == Item.ItemType.SHIELD ? true : false);
            hasWeapon = (player.current_item.GetComponent<Item>().item_type == Item.ItemType.WEAPON ? true : false);
        }

        if (!hasShield)
        {
            int newPlayerHP = player.HP - enemy.damage;
            player.HP = newPlayerHP;
            if (newPlayerHP <= 0)
            {
                return Enums.CombatResult.PLAYER_DIED;
            }
        }

        if (hasWeapon)
        {
            enemy.HP = 0;
            return Enums.CombatResult.ENEMY_DIED;
        }
        int newEnemyHP = enemy.HP - 1;
        enemy.HP = newEnemyHP;
        if (newEnemyHP <= 0)
        {
            return Enums.CombatResult.ENEMY_DIED;
        }

        if (hasShield)
            return Enums.CombatResult.SHIELD_DEFEND;
        return Enums.CombatResult.CLASH;
    }

    public void StunnedByBatty()
    {
        player.HP -= 2;
        if (player.HP <= 0)
        {
            player.die_flag = true;
            return;
        }

        // Stunend for one turn
        FindObjectOfType<EventSystem>().PlayerPerformedEvent();

        // Cam shake
        FindObjectOfType<CameraShake>().DoShake(Constants.MediumCamShake);

        // Start the stun coroutine
        StartCoroutine("StunCoroutine");
    }

    public void RemoveShield()
    {
        // Play sfx 
        sfx.PlaySFX("enemy_hurt");
        sfx.PlaySFX("armor_break");
        cam_shake.DoShake(Constants.LightCamShake);
        // Remove shield
        player.RemoveCurrentItem();
    }

    private IEnumerator StunCoroutine()
    {
        player.BlockInput();
        spre.color = Color.red;

        yield return new WaitForSeconds(Constants.StunTime);


        spre.color = Color.white;
        player.UnblockInput();
    }

}
