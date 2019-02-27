using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour 
{
   
    /*
    public static CombatResult PerformCombat(Player _player, Enemy _enemy)
    {
        // Does the player have a shield?
        bool hasShield = (_player.current_item != null ? (_player.current_item.GetComponent<Item>().item_type == Item.ItemType.SHIELD ? true : false) : false);
        bool hasWeapon = (_player.current_item != null ? (_player.current_item.GetComponent<Item>().item_type == Item.ItemType.WEAPON ? true : false) : false);

        if (!hasShield)
        {
            int newPlayerHP = _player.HP - _enemy.damage;
            _player.HP = newPlayerHP;
            if (newPlayerHP <= 0)
            {
                return CombatResult.PLAYER_DIED;
            }
        }

        if (hasWeapon)
        {
            _enemy.HP = 0;
            return CombatResult.ENEMY_DIED;
        }
        int newEnemyHP = _enemy.HP - 1;
        _enemy.HP = newEnemyHP;
        if(newEnemyHP <= 0)
        {
            return CombatResult.ENEMY_DIED;
        }

        if (hasShield)
            return CombatResult.SHIELD_DEFEND;
        return CombatResult.CLASH;
    }
    */

}
