using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    // All the objects that should be notified on "action" aka "player did something"
    private Player player;
    private GameMaster game_master; // Enemies from current_level.enemy_map
    private SpikeSystem spike_system;
    
    private void Awake()
    {
        spike_system = FindObjectOfType<SpikeSystem>();
        game_master = FindObjectOfType<GameMaster>();
        player = FindObjectOfType<Player>();
    }

    public void PlayerPerformedEvent()
    {
        // Update all the enemies 
        List<Enemy> enemyList = new List<Enemy>();
        foreach(var x in game_master.current_map.enemy_map)
        {
            game_master.current_map.tile_map[(int)x.Value.tile_position.y, (int)x.Value.tile_position.x] = (int)x.Value.tile_value_under;

            // This updates the position of the enemy! 
            x.Value.PlayerEvent(); 

            enemyList.Add(x.Value);

            // What are we standing on now?
            x.Value.tile_value_under = (MapManager.TileValues)game_master.current_map.tile_map[(int)x.Value.tile_position.y, (int)x.Value.tile_position.x];
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

        // Message spikes
        spike_system.Step();
        // Check if the player is on a spike system and if the player should die!
        if (game_master.current_map.spike_map.ContainsKey(player.tile_position))
        {
            if (spike_system.spikeLevel == 0)
                player.TakeSpikeHit();
        }

        // Update game master
        game_master.PlayerEvent();
    }
}
