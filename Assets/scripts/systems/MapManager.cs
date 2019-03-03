using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public static float GroundTileSize;

    public GameObject skeleton_prefab;
    public GameObject spike_prefab;
    [Header("Gate Prefabs")]
    public GameObject gate_right_red;
    public GameObject gate_left_red;
    public GameObject gate_forward_red;
    public GameObject gate_upward_red;
    public GameObject gate_right_blue;
    public GameObject gate_left_blue;
    public GameObject gate_forward_blue;
    public GameObject gate_upward_blue;

    [Header("File name of the level to load!")]
    public string level_name;

    public enum TileValues
    {
        // Room kit
        GROUND = 30,
        LEFT_WALL = 1,
        RIGHT_WALL = 2,
        TOP_WALL = 3,
        PIT = 4,
        BOT_LEFT_WALL = 5,
        BOT_RIGHT_WALL = 6,
        // Items
        WEAPON = 7,
        SHIELD = 8,
        HEALTH = 9,
        // Enemy
        SKELETON = 31,

        // MAP ELEMENTS
        GOAL = 11,
        SPIKE = 12,
        // GATES
        GATE_RIGHT_RED = 13,
        GATE_LEFT_RED = 14,
        GATE_FORWARD_RED = 15,
        GATE_UPWARD_RED = 16,
        GATE_RIGHT_BLUE = 17,
        GATE_LEFT_BLUE = 18,
        GATE_FORWARD_BLUE = 19,
        GATE_UPWARD_BLUE = 20,

        NONE = -1
    };

    bool ground_trigger = false;

    public GameObject map_holder;

    void Awake()
    {
        GroundTileSize = ResourceLoader.GetSprite("floor0").bounds.size.x;
    }

    public Map SpawnMap()
    {
        GroundTileSize = ResourceLoader.GetSprite("floor0").bounds.size.x;

        Map map = new Map();
        //map.tile_map = map1;
        map.tile_map = TiledImporter.LoadTiledMap(level_name);

        // Clear stuff
        foreach (Transform go in map_holder.transform) {
            Destroy(go.gameObject);
        }
        for (int x = 0; x < Constants.MapWidth; x++)
        {
            for (int y = 0; y < Constants.MapHeight; y++)
            {
                int tile_value = map.tile_map[y, x];
                GameObject createdGround = null;
                GameObject createdItemEnemy = null;
                switch (tile_value)
                {
                    case (int)TileValues.GROUND:
                        createdGround = CreateGround(x, y);
                        break;
                    case (int)TileValues.LEFT_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("wallLeft");
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.RIGHT_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("wallRight");
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.TOP_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "top_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("wallTop");
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.PIT:
                        GameObject pit = new GameObject();
                        pit.name = "pit(" + x.ToString() + ", " + y.ToString() + ")";
                        pit.AddComponent<SpriteRenderer>();
                        pit.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("pit");
                        pit.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        pit.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.BOT_LEFT_WALL:
                        GameObject bot_left_wall = new GameObject();
                        bot_left_wall.name = "bot_left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        bot_left_wall.AddComponent<SpriteRenderer>();
                        bot_left_wall.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("bottomLeftWall");
                        bot_left_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        bot_left_wall.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.BOT_RIGHT_WALL:
                        GameObject bot_right_wall = new GameObject();
                        bot_right_wall.name = "bot_right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        bot_right_wall.AddComponent<SpriteRenderer>();
                        bot_right_wall.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("bottomRightWall");
                        bot_right_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        bot_right_wall.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.WEAPON:
                        // First spawn ground
                        createdGround = CreateGround(x, y);

                        // Spawn weapon object
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "weapon";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("weapon");
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        // Item stuff
                        createdItemEnemy.AddComponent<Item>();
                        createdItemEnemy.GetComponent<Item>().item_type = Item.ItemType.WEAPON;
                        map.item_map.Add(new Vector2(x, y), createdItemEnemy);
                        break;
                    case (int)TileValues.SHIELD:
                        // First spawn ground
                        createdGround = CreateGround(x, y);

                        // Spawn shield object
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "shield";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("shield");
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        // Item stuff
                        createdItemEnemy.AddComponent<Item>();
                        createdItemEnemy.GetComponent<Item>().item_type = Item.ItemType.SHIELD;
                        map.item_map.Add(new Vector2(x, y), createdItemEnemy);
                        break;
                    case (int)TileValues.HEALTH:
                        createdGround = CreateGround(x, y);

                        // Spawn health object
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "health";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("food");
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        // Item stuff
                        createdItemEnemy.AddComponent<Item>();
                        createdItemEnemy.GetComponent<Item>().item_type = Item.ItemType.HEALTH;
                        map.item_map.Add(new Vector2(x, y), createdItemEnemy);
                        break;
                    case (int)TileValues.SKELETON:
                        createdGround = CreateGround(x, y);
                        // Spawn skeleton
                        createdItemEnemy = Instantiate(skeleton_prefab);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize);
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        map.enemy_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Enemy>());
                        break;
                    case (int)TileValues.GOAL:
                        GameObject go1 = new GameObject();
                        go1.name = "GOAL";
                        go1.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        go1.AddComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("goalInner");
                        go1.GetComponent<SpriteRenderer>().sortingOrder = y - 1;
                        GameObject go2 = new GameObject();
                        go2.name = "GOAL";
                        go2.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        go2.AddComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("goalOuter");
                        go2.GetComponent<SpriteRenderer>().sortingOrder = y + 1;

                        go1.transform.SetParent(map_holder.transform);
                        go2.transform.SetParent(map_holder.transform);

                        break;
                    case (int)TileValues.SPIKE:
                        createdGround = CreateGround(x, y);
                        // Spawn spike
                        createdItemEnemy = Instantiate(spike_prefab);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize*0.5f);
                        createdItemEnemy.GetComponent<SpriteRenderer>().sortingOrder = y;
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        map.spike_map.Add(new Vector2(x, y), createdItemEnemy);
                        break;
                    case (int)TileValues.GATE_LEFT_RED:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_left_red);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.5f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<KeyGate>());

                        break;
                    case (int)TileValues.GATE_RIGHT_RED:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_right_red);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.5f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<KeyGate>());

                        break;
                    case (int)TileValues.GATE_FORWARD_RED:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_forward_red);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.5f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<KeyGate>());

                        break;
                    case (int)TileValues.GATE_UPWARD_RED:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_upward_red);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.5f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<KeyGate>());

                        break;
                }

                // Layer the ground
                if(createdGround != null)
                {
                    createdGround.GetComponent<SpriteRenderer>().sortingLayerName = "room_tiles";
                }
                // Layer the enemy/item
                if(createdItemEnemy != null)
                {
                    createdItemEnemy.GetComponent<SpriteRenderer>().sortingLayerName = "player_items_enemies";
                    createdItemEnemy.GetComponent<SpriteRenderer>().sortingOrder = Constants.MapHeight - y;
                }

                ground_trigger = !ground_trigger;
            }
        }

        return map;
    }

    private GameObject CreateGround(int x, int y)
    {
        GameObject createdGround = new GameObject();
        createdGround.name = "ground(" + x.ToString() + ", " + y.ToString() + ")";
        createdGround.AddComponent<SpriteRenderer>();
        createdGround.GetComponent<SpriteRenderer>().sprite = ground_trigger ? ResourceLoader.GetSprite("floor0") : ResourceLoader.GetSprite("floor1");
        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
        createdGround.transform.SetParent(map_holder.transform);

        return createdGround;
    }

    public static bool IsWalkable(int tile_value)
    {
        if(tile_value == (int)TileValues.GROUND)
        {
            return true;
        }

        return false;
    }

    public static bool IsItem(int tile_value)
    {
        if(tile_value == (int)TileValues.WEAPON || tile_value == (int)TileValues.SHIELD || tile_value == (int)TileValues.HEALTH)
        {
            return true;
        }

        return false;
    }

    public static bool IsEnemy(int tile_value)
    {
        if (tile_value == (int)TileValues.SKELETON)
            return true;
        return false;
    }

    public static bool IsGate(int tile_value)
    {
        switch (tile_value)
        {
            case (int)TileValues.GATE_FORWARD_BLUE:
            case (int)TileValues.GATE_FORWARD_RED:
            case (int)TileValues.GATE_LEFT_BLUE:
            case (int)TileValues.GATE_LEFT_RED:
            case (int)TileValues.GATE_RIGHT_BLUE:
            case (int)TileValues.GATE_RIGHT_RED:
            case (int)TileValues.GATE_UPWARD_BLUE:
            case (int)TileValues.GATE_UPWARD_RED:
                return true;

            default:
                return false;
        }
    }

    /*
     * Used in the importer to check if tile_value1 should be replaced by tile_value2
    */
    public static bool ShouldReplace(int tile_value1, int tile_value2)
    {
        // If tile_value2 is enemy or item and tile_value1 is walkable then replace 
        if( IsWalkable(tile_value1) && (IsEnemy(tile_value2) || IsItem(tile_value2) || tile_value2 == (int)TileValues.SPIKE || IsGate(tile_value2) ))
        {
            return true;
        }
        return false;
    }

}
