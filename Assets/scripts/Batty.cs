using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batty : Enemy
{
    // They have different walking patterns
    public enum BattyType
    {
        ThreeBySeven = MapManager.TileValues.BATTY1,
        FiveByFive = MapManager.TileValues.BATTY2
    }

    int counter = 0;
    int screech_counter = 0;

    [System.NonSerialized]
    public BattyType batType;

    // Directions list
    static List<Vector2> directionList1 = new List<Vector2>();
    static List<Vector2> directionList2 = new List<Vector2>();

    [System.NonSerialized]
    public Vector2 start_tile_pos;

    private void Start()
    {
        directionList1.Add(new Vector2(0, 0));
        directionList1.Add(new Vector2(-2, 0));
        directionList1.Add(new Vector2(-2, -2));
        directionList1.Add(new Vector2(-2, -4));
        directionList1.Add(new Vector2(0, -4));
        directionList1.Add(new Vector2(2, -4));
        directionList1.Add(new Vector2(2, -2));
        directionList1.Add(new Vector2(2, 0));

        directionList2.Add(new Vector2(0, 0));
        directionList2.Add(new Vector2(-1, -1));
        directionList2.Add(new Vector2(-1, -3));
        directionList2.Add(new Vector2(-1, -5));
        directionList2.Add(new Vector2(0, -6));
        directionList2.Add(new Vector2(1, -5));
        directionList2.Add(new Vector2(1, -3));
        directionList2.Add(new Vector2(1, -1));

        BaseStart();
    }

    private void Update()
    {
        if ((Vector2)transform.position != (tile_position * MapManager.GroundTileSize) + (Vector2)Offsets.BatEnemyOffset)
        {
            transform.position = Vector2.MoveTowards(transform.position, (new Vector3(tile_position.x, tile_position.y, 0) * MapManager.GroundTileSize) + Offsets.BatEnemyOffset, Constants.BattyMoveSpeed * Time.deltaTime);
        }
    }

    public override void TakeDamage()
    {
        // 1 hp
        HP = 0;
    }

    public override void PlayerEvent()
    {
        // Check if we screechs
        if (screech_counter == 3)
        {
            print("REEEEEEEEE");
            screech_counter = 0;
        }
        else
        {
            // Increment screechers
            screech_counter++;

            // Step counter
            if (counter == 7)
            {
                counter = 0;
            }
            else
            {
                counter++;
            }

            if (batType == BattyType.FiveByFive)
            {
                // Move here using tile_position
                tile_position = start_tile_pos + directionList1[counter];
            }
            else // ThreeBySeven
            {
                tile_position = start_tile_pos + directionList2[counter];
            }

            spre.sortingOrder = Constants.MapHeight - (int)tile_position.y + 10;
        }
    }

}
