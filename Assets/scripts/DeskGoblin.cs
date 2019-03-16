using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskGoblin : Enemy
{

    public Sprite damaged_sprite, dead_sprite;
    int move_counter = 0;
    bool toggler = false;

    public void Start() 
    {
        base.BaseStart();
    }

    void Update()
    {
        if((Vector2)transform.position != tile_position * MapManager.GroundTileSize)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(tile_position.x, tile_position.y, 0) * MapManager.GroundTileSize, Constants.ObjectMoveSpeed * Time.deltaTime);
        }
    }

    public override void TakeDamage()
    {
        HP--;
        // TODO@ Fix this 
        /*
        if (HP == 2)
        {
            spre.sprite = damaged_sprite;
        }
        else
        {
            spre.sprite = dead_sprite;
        }
        */
    }

    public override void Step()
    {
        /*
        move_counter++;
        if (move_counter == 4)
        {
            // Move the desk goblin accordingly!

            //game_master.current_map.enemy_map.Remove(tile_position);
            game_master.current_map.tile_map[(int)tile_position.y, (int)tile_position.x] = (int)MapManager.TileValues.GROUND;

            if(toggler)
                tile_position = new Vector2(tile_position.x, tile_position.y + 1);
            else
                tile_position = new Vector2(tile_position.x, tile_position.y - 1);
            game_master.current_map.tile_map[(int)tile_position.y, (int)tile_position.x] = (int)tile_value;

            // Add to the map
            //game_master.current_map.enemy_map.Add(tile_position, this);

            toggler = !toggler;
            move_counter = 0;
        }
        */
    }
}
