using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemy : Enemy
{
    public enum GuardDirection
    {
        UP = MapManager.TileValues.GUARD_UP,
        LEFT = MapManager.TileValues.GUARD_LEFT
    }

    [System.NonSerialized]
    public GuardDirection guardDirection;

    private Vector2 walk_direction;

    private void Start()
    {
        BaseStart();

        if(guardDirection == GuardDirection.UP)
        {
            walk_direction = new Vector2(0, -1);
        }
        else if(guardDirection == GuardDirection.LEFT)
        {
            walk_direction = new Vector2(-1, 0);
        }
    }

    private void Update()
    {
        if ((Vector2)transform.position != (tile_position * MapManager.GroundTileSize) + (Vector2)Offsets.GuardEnemyOffset)
        {
            transform.position = Vector2.MoveTowards(transform.position, (new Vector3(tile_position.x, tile_position.y, 0) * MapManager.GroundTileSize) + Offsets.GuardEnemyOffset, Constants.ObjectMoveSpeed * Time.deltaTime);
        }
    }

    public override void PlayerEvent()
    {
        Vector2 new_tile_position = tile_position + walk_direction;
        int new_tile_value = game_master.current_map.tile_map[(int)new_tile_position.y, (int)new_tile_position.x];

        if (MapManager.IsWalkable(new_tile_value))
        {
            tile_position = new_tile_position;
        }
        else
        {
            walk_direction = walk_direction * -1; // Change direction
            // Try to walk now 
            new_tile_position = tile_position + walk_direction;
            new_tile_value = game_master.current_map.tile_map[(int)new_tile_position.y, (int)new_tile_position.x];
            if (MapManager.IsWalkable(new_tile_value))
            {
                tile_position = new_tile_position;
            }
        }

    }

}
