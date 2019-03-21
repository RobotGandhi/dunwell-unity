using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        WEAPON = 27,
        SHIELD = 28,
        HEALTH = 29,
        RED_KEY = 30,
        BLUE_KEY = 31
    }

    public enum ItemState
    {
        ON_MAP,
        PICKED_UP,
        DISCARDED_FROM_MAP
    }

    [System.NonSerialized]
    public ItemType item_type;
    public Vector2 spawn_tile_position;

    public void ResetPosition()
    {
        transform.position = new Vector3(spawn_tile_position.x, spawn_tile_position.y, 0) * MapManager.GroundTileSize;
        GetComponent<SpriteRenderer>().sortingLayerName = "player_items_enemies";
        GetComponent<SpriteRenderer>().sortingOrder = Constants.MapHeight - (int)spawn_tile_position.y - 1;
    }
}
