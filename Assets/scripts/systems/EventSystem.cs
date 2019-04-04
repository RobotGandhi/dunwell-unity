using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    // All the objects that should be notified on "action" aka "player did something"
    private Player player;
    private GameMaster game_master; // Enemies from current_level.enemy_map
    
    private void Start()
    {
        game_master = FindObjectOfType<GameMaster>();
    }

    public void PlayerPerformedEvent()
    {
        // Update all the enemies 
        List<Enemy> enemyList = new List<Enemy>();
        foreach(var x in game_master.current_map.enemy_map)
        {
            game_master.current_map.tile_map[(int)x.Value.tile_position.y, (int)x.Value.tile_position.x] = (int)MapManager.TileValues.GROUND;
            x.Value.PlayerEvent(); // This updates the position of the enemy! 
            enemyList.Add(x.Value);
        }
        Dictionary<Vector2, Enemy> newEnemyMap = new Dictionary<Vector2, Enemy>();
        foreach(Enemy enemy in enemyList)
        {
            if (newEnemyMap.ContainsKey(enemy.tile_position))
            {
                newEnemyMap.Add(enemy.tile_position + Vector2.up * 100, enemy);
            }
            else
            {
                newEnemyMap.Add(enemy.tile_position, enemy);
            }

            game_master.current_map.tile_map[(int)enemy.tile_position.y, (int)enemy.tile_position.x] = (int)enemy.tile_value;

        }

        game_master.current_map.enemy_map = newEnemyMap;

        // Update game master
        game_master.PlayerEvent();
    }
}
