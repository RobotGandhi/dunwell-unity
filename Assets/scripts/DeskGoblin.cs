using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskGoblin : Enemy
{

    public Sprite damaged_sprite, dead_sprite;

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
    }

    public override void PlayerEvent()
    {
          
    }
}
