using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemy : Enemy
{
    public enum GuardDirection
    {
        UP = MapManager.TileValues.GUARD_UP,
        LEFT = MapManager.TileValues.GUARD_LEFT,
        RIGHT = MapManager.TileValues.GUARD_RIGHT,
        DOWN = MapManager.TileValues.GUARD_DOWN,
    }

    [System.NonSerialized]
    public GuardDirection guardDirection;

    private Vector2 walk_direction;

    int walk_counter = 0;

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
        else if(guardDirection == GuardDirection.RIGHT)
        {
            walk_direction = new Vector2(1, 0);
        }
        else if(guardDirection == GuardDirection.DOWN)
        {
            walk_direction = new Vector2(0, 1);
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

        // Walking logic for the guard
        walk_counter++;
        if (walk_counter == 5)
        {
            walk_direction *= -1;
            walk_counter = 0;
        }
        else if (MapManager.IsWalkable(new_tile_value) || new_tile_value == (int)MapManager.TileValues.SPIKE)
        {
            tile_position = new_tile_position;
        }
        else if (MapManager.IsGate(new_tile_value))
        {
            if (game_master.current_map.gate_map[new_tile_position].IsOpen())
                tile_position = new_tile_position;
            else
            {
                walk_counter = 0;
                walk_direction = walk_direction * -1;
            }
        }
        else
        {
            walk_counter = 0;
            walk_direction = walk_direction * -1; // Change direction
        }

        // Clumsy logic code for if the guard sees and should X the player

        Vector2 p = player.tile_position;

        if (walk_direction == new Vector2(1, 0))
        {
            Vector2 right = tile_position + new Vector2(1,0);
            Vector2 right2 = tile_position + new Vector2(2, 0);
            Vector2 down = tile_position + new Vector2(1, -1);
            Vector2 up = tile_position + new Vector2(1, 1);

            if (p == right || p == up || p == down)
            {
                player.die_flag = true;
                return;
            }
            else if (p == right2)
            {
                // Check to see that down1 isn't obstructing our view
                if (!IsObstruction(right2))
                {
                    player.die_flag = true;
                    return;
                }
            }

        }
        else if (walk_direction == new Vector2(-1, 0))
        {
            Vector2 left = tile_position + new Vector2(-1, 0);
            Vector2 left2 = tile_position + new Vector2(-2, 0);
            Vector2 down = tile_position + new Vector2(-1, -1);
            Vector2 up = tile_position + new Vector2(-1, 1);

            if (p == left || p == up || p == down)
            {
                player.die_flag = true;
                return;
            }
            else if (p == left2)
            {
                // Check to see that down1 isn't obstructing our view
                if (!IsObstruction(left))
                {
                    player.die_flag = true;
                    return;
                }
            }
        }
        else if (walk_direction == new Vector2(0, 1))
        {
            Vector2 left = tile_position + new Vector2(-1,  1);
            Vector2 right = tile_position + new Vector2(1,  1);
            Vector2 down1 = tile_position + new Vector2(0,  1);
            Vector2 down2 = tile_position + new Vector2(0,  2);

            if (p == left || p == right || p == down1)
            {
                player.die_flag = true;
                return;
            }
            else if (p == down2)
            {
                // Check to see that down1 isn't obstructing our view
                if (!IsObstruction(down1))
                {
                    player.die_flag = true;
                    return;
                }
            }
        }
        else if (walk_direction == new Vector2(0, -1))
        {
            Vector2 left = tile_position + new Vector2(-1, -1);
            Vector2 right = tile_position + new Vector2(1, -1);
            Vector2 down1 = tile_position + new Vector2(0, -1);
            Vector2 down2 = tile_position + new Vector2(0, -2);

            if(p == left || p == right || p == down1)
            {
                player.die_flag = true;
                return;
            }
            else if(p == down2)
            {
                // Check to see that down1 isn't obstructing our view
                if (!IsObstruction(down1))
                {
                    player.die_flag = true;
                    return;
                }
            }
        }

        spre.sortingOrder = Constants.MapHeight - (int)tile_position.y;
    }

    private bool IsObstruction(Vector2 tile_pos)
    {
        int tile_value = game_master.current_map.tile_map[(int)tile_pos.y, (int)tile_pos.x];

        if (tile_value == (int)MapManager.TileValues.BLOCK)
            return true;

        if(game_master.current_map.gate_map.ContainsKey(tile_pos))
        {
            var gate = game_master.current_map.gate_map[tile_pos];
            if (!gate.IsOpen())
                return true;
        }

        return false;

    }
}
