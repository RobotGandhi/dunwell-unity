using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Sprite TopLeftWall, TopWall, TopRightWall, Ground, BotBotLeftWall, BotBotRightWall, BotLeftWall, BotRightWall, Pit, RightWall, LeftWall, GroundCracked1, GroundCracked2, GroundCracked3, GroundCracked4, GroundCracked5;
    public Sprite WoodSpike, BlockSprite, WoodSpikeStartVertical, WoodSpikeConnectorVertical, WoodSpikeEndVertical;
    public Sprite BlueKey, RedKey;

    public Sprite PPGroundRL;
    public Sprite PPGroundLD;
    public Sprite PPGroundUR;
    public Sprite PPGroundUL;
    public Sprite PPGroundLR;
    public Sprite PPGroundUD;

    public Sprite GroundMossy1, GroundMossy2, GroundMossy3, GroundMossy4, GroundMossy5;
    public Sprite LeftWallMossy, RightWallMossy, TopWallMossy;

    [Header("Item Sprites")]
    public Sprite WeaponSprite;
    public Sprite ShieldSprite;
    public Sprite HealthSprite;

    [Header("Other")]
    public Sprite GoalSprite1;
    public GameObject PresurePlatePrefab;

    [Header("Hazards")]
    public GameObject spike_prefab_single;

    public static float GroundTileSize;

    [Header("Enemies")]
    public GameObject DeskGoblinPrefab;
    public GameObject GuardPrefab;

    [Header("Gate Prefabs")]
    public GameObject gate_side_red;
    public GameObject gate_forward_red;
    public GameObject gate_side_blue;
    public GameObject gate_forward_blue;
    public GameObject pp_gate_forward;
    public GameObject pp_gate_side;

    public enum TileValues
    {
        // Room kit
        GROUND = 8,
        LEFT_WALL = 5,
        RIGHT_WALL = 4,
        TOP_WALL = 2,
        TOP_LEFT_WALL = 1,
        TOP_RIGHT_WALL = 3,
        PIT = 10,
        BOT_LEFT_WALL = 6,
        BOT_RIGHT_WALL = 7,
        BOT_BOT_LEFT_WALL = 9,
        BOT_BOT_RIGHT_WALL = 11,
        GROUND_CRACKED1 = 12,
        GROUND_CRACKED2 = 13,
        GROUND_CRACKED3 = 14,
        GROUND_CRACKED4 = 15,
        GROUND_CRACKED5 = 16,
        GROUND_MOSSY1 = 20,
        GROUND_MOSSY2 = 21,
        GROUND_MOSSY3 = 22,
        GROUND_MOSSY4 = 23,
        GROUND_MOSSY5 = 24,
        TOP_WALL_MOSSY = 18,
        LEFT_WALL_MOSSY = 17,
        RIGHT_WALL_MOSSY = 19,
        PP_GROUND_RL = 67,
        PP_GROUND_LD = 68,
        PP_GROUND_UR = 69,
        PP_GROUND_UL = 70,
        PP_GROUND_LR = 71,
        PP_GROUND_UD = 72,

        BLOCK = 64,

        // Items
        WEAPON = 27,
        SHIELD = 28,
        HEALTH = 29,
        BLUE_KEY = 30,
        RED_KEY = 31,

        // Enemy
        DESKGOBLIN = 58,
        GUARD_LEFT = 59,
        GUARD_UP = 60,

        // MAP ELEMENTS
        GOAL1 = 54,
        PRESURE_PLATE = 25,
        SPIKE = 32,

        FALL_SPIKE = 33,
        FALL_SPIKE_START_VERTICAL = 35,
        FALL_SPIKE_CONNECTOR_VERTICAL = 36,
        FALL_SPIKE_ENDER_VERTICAL = 34,
        
        // GATES
        GATE_FORWARD_RED = 48,
        GATE_FORWARD_BLUE = 46,
        GATE_SIDE_RED = 47,
        GATE_SIDE_BLUE = 45,
        PP_GATE_FORWARD = 74,
        PP_GATE_SIDE = 73,

        NONE = -1
    };

    bool ground_trigger = false;

    public GameObject map_holder;

    void Awake()
    {
        GroundTileSize = Ground.bounds.size.x;
    }

    public Map SpawnMap(string level_name)
    {
        GroundTileSize = Ground.bounds.size.x;

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
                    case (int)TileValues.GROUND_CRACKED1:
                    case (int)TileValues.GROUND_CRACKED2:
                    case (int)TileValues.GROUND_CRACKED3:
                    case (int)TileValues.GROUND_CRACKED4:
                    case (int)TileValues.GROUND_CRACKED5:
                    case (int)TileValues.PP_GROUND_RL:
                    case (int)TileValues.PP_GROUND_LD:
                    case (int)TileValues.PP_GROUND_UR:
                    case (int)TileValues.PP_GROUND_UL:
                    case (int)TileValues.PP_GROUND_LR:
                    case (int)TileValues.PP_GROUND_UD:
                    case (int)TileValues.GROUND_MOSSY1:
                    case (int)TileValues.GROUND_MOSSY2:
                    case (int)TileValues.GROUND_MOSSY3:
                    case (int)TileValues.GROUND_MOSSY4:
                    case (int)TileValues.GROUND_MOSSY5:
                        createdGround = CreateGround(x, y);
                        break;

                    case (int)TileValues.LEFT_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = LeftWall;
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.RIGHT_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = RightWall;
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.TOP_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "top_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = TopWall;
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.LEFT_WALL_MOSSY:
                        createdGround = new GameObject();
                        createdGround.name = "right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = LeftWallMossy;
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.RIGHT_WALL_MOSSY:
                        createdGround = new GameObject();
                        createdGround.name = "right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = RightWallMossy;
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.TOP_WALL_MOSSY:
                        createdGround = new GameObject();
                        createdGround.name = "right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = TopWallMossy;
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.PIT:
                        GameObject pit = new GameObject();
                        pit.name = "pit(" + x.ToString() + ", " + y.ToString() + ")";
                        pit.AddComponent<SpriteRenderer>();
                        pit.GetComponent<SpriteRenderer>().sprite = Pit;
                        pit.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        pit.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.BOT_LEFT_WALL:
                        GameObject bot_left_wall = new GameObject();
                        bot_left_wall.name = "bot_left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        bot_left_wall.AddComponent<SpriteRenderer>();
                        bot_left_wall.GetComponent<SpriteRenderer>().sprite = BotLeftWall;
                        bot_left_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        bot_left_wall.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.BOT_RIGHT_WALL:
                        GameObject bot_right_wall = new GameObject();
                        bot_right_wall.name = "bot_right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        bot_right_wall.AddComponent<SpriteRenderer>();
                        bot_right_wall.GetComponent<SpriteRenderer>().sprite = BotRightWall;
                        bot_right_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        bot_right_wall.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.BOT_BOT_LEFT_WALL:
                        GameObject botbot_left_wall = new GameObject();
                        botbot_left_wall.name = "botbot_left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        botbot_left_wall.AddComponent<SpriteRenderer>();
                        botbot_left_wall.GetComponent<SpriteRenderer>().sprite = BotBotLeftWall;
                        botbot_left_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        botbot_left_wall.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.BOT_BOT_RIGHT_WALL:
                        GameObject botbot_right_wall = new GameObject();
                        botbot_right_wall.name = "botbot_right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        botbot_right_wall.AddComponent<SpriteRenderer>();
                        botbot_right_wall.GetComponent<SpriteRenderer>().sprite = BotBotRightWall;
                        botbot_right_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        botbot_right_wall.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.TOP_LEFT_WALL:
                        GameObject top_left = new GameObject();
                        top_left.name = "top_left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        top_left.AddComponent<SpriteRenderer>();
                        top_left.GetComponent<SpriteRenderer>().sprite = TopLeftWall;
                        top_left.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        top_left.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.TOP_RIGHT_WALL:
                        GameObject top_right = new GameObject();
                        top_right.name = "top_right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        top_right.AddComponent<SpriteRenderer>();
                        top_right.GetComponent<SpriteRenderer>().sprite = TopRightWall;
                        top_right.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        top_right.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.BLOCK:
                        GameObject block = new GameObject();
                        block.name = "block";
                        block.AddComponent<SpriteRenderer>().sprite = BlockSprite;
                        block.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize*0.25f, 0);
                        block.transform.SetParent(map_holder.transform);

                        block.GetComponent<SpriteRenderer>().sortingLayerName = "player_items_enemies";
                        block.GetComponent<SpriteRenderer>().sortingOrder = Constants.MapHeight - y;

                        break;
                    case (int)TileValues.WEAPON:
                        // First spawn ground
                        createdGround = CreateGround(x, y);

                        // Spawn weapon object
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "weapon";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = WeaponSprite;
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
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = ShieldSprite;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        // Item stuff
                        createdItemEnemy.AddComponent<Item>();
                        createdItemEnemy.GetComponent<Item>().item_type = Item.ItemType.SHIELD;
                        map.item_map.Add(new Vector2(x, y), createdItemEnemy);
                        break;
                    case (int)TileValues.HEALTH:                        // First spawn ground
                        createdGround = CreateGround(x, y);

                        // Spawn health object
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "health";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = HealthSprite;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        // Item stuff
                        createdItemEnemy.AddComponent<Item>();
                        createdItemEnemy.GetComponent<Item>().item_type = Item.ItemType.HEALTH;
                        map.item_map.Add(new Vector2(x, y), createdItemEnemy);
                        break;
                    case (int)TileValues.DESKGOBLIN:
                        createdGround = CreateGround(x, y);

                        // Spawn enemy
                        createdItemEnemy = Instantiate(DeskGoblinPrefab);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize);
                        createdItemEnemy.GetComponent<SpriteRenderer>().sortingOrder = y;
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        map.enemy_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Enemy>());
                        map.enemy_map[new Vector2(x, y)].tile_position = new Vector2(x, y);
                        map.enemy_map[new Vector2(x, y)].tile_value = (TileValues)tile_value;
                        break;
                    case (int)TileValues.GOAL1:
                        createdGround = new GameObject();
                        createdGround.name = "GOAL";
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.AddComponent<SpriteRenderer>().sprite = GoalSprite1;
                        createdGround.transform.SetParent(map_holder.transform);
                        map.goal = createdGround;
                        break;
                    case (int)TileValues.SPIKE:
                        createdGround = CreateGround(x, y);
                        // Spawn spike
                        createdItemEnemy = Instantiate(spike_prefab_single);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize*0.5f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        map.spike_map.Add(new Vector2(x, y), createdItemEnemy);
                        break;
                    case (int)TileValues.GATE_SIDE_RED:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_side_red);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.5f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Gate>());

                        break;
                    case (int)TileValues.GATE_FORWARD_RED:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_forward_red);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.25f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Gate>());

                        break;
                    case (int)TileValues.GATE_SIDE_BLUE:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_side_blue);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.5f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Gate>());
                        break;
                    case (int)TileValues.GATE_FORWARD_BLUE:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(gate_forward_blue);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.25f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Gate>());
                        break;
                    case (int)TileValues.PP_GATE_FORWARD:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(pp_gate_forward);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.25f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Gate>());
                        break;
                    case (int)TileValues.PP_GATE_SIDE:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = Instantiate(pp_gate_side);
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) + GroundTileSize * 0.25f);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.gate_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<Gate>());
                        break;
                    case (int)TileValues.FALL_SPIKE:
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "wood_spike(" + x.ToString() + ", " + y.ToString() + ")";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = WoodSpike;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.FALL_SPIKE_START_VERTICAL:
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "wood_spike_vertical_start";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = WoodSpikeStartVertical;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) - GroundTileSize*0.25f, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.FALL_SPIKE_CONNECTOR_VERTICAL:
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "wood_spike_vertical_connector";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = WoodSpikeConnectorVertical;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, (y * GroundTileSize) - GroundTileSize*0.25f, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.FALL_SPIKE_ENDER_VERTICAL:
                        createdItemEnemy = new GameObject();
                        createdItemEnemy.name = "wood_spike_vertical_ender";
                        createdItemEnemy.AddComponent<SpriteRenderer>();
                        createdItemEnemy.GetComponent<SpriteRenderer>().sprite = WoodSpikeEndVertical;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);
                        break;
                    case (int)TileValues.RED_KEY:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = new GameObject("red key");
                        createdItemEnemy.AddComponent<SpriteRenderer>().sprite = RedKey;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        createdItemEnemy.AddComponent<Item>();
                        createdItemEnemy.GetComponent<Item>().item_type = Item.ItemType.RED_KEY;
                        createdItemEnemy.GetComponent<Item>().spawn_tile_position = new Vector2(x, y);
                        map.item_map.Add(new Vector2(x, y), createdItemEnemy);

                        break;
                    case (int)TileValues.BLUE_KEY:
                        createdGround = CreateGround(x, y);

                        createdItemEnemy = new GameObject("blue key");
                        createdItemEnemy.AddComponent<SpriteRenderer>().sprite = BlueKey;
                        createdItemEnemy.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        createdItemEnemy.AddComponent<Item>();
                        createdItemEnemy.GetComponent<Item>().item_type = Item.ItemType.BLUE_KEY;
                        createdItemEnemy.GetComponent<Item>().spawn_tile_position = new Vector2(x, y);
                        map.item_map.Add(new Vector2(x, y), createdItemEnemy);

                        break;
                    case (int)TileValues.PRESURE_PLATE:
                        createdGround = CreateGround(x, y);
                        createdItemEnemy = Instantiate(PresurePlatePrefab, new Vector3(x * GroundTileSize, (y * GroundTileSize), 0), Quaternion.identity);
                        createdItemEnemy.transform.SetParent(map_holder.transform);

                        map.pp_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<PresurePlate>());

                        break;
                    case (int)TileValues.GUARD_LEFT:
                    case (int)TileValues.GUARD_UP:
                        createdGround = CreateGround(x, y);
                        createdItemEnemy = Instantiate(GuardPrefab, new Vector3(x * GroundTileSize, y * GroundTileSize, 0) + Offsets.GuardEnemyOffset, Quaternion.identity);

                        // Direction!
                        createdItemEnemy.GetComponent<GuardEnemy>().guardDirection = (GuardEnemy.GuardDirection)tile_value;

                        map.enemy_map.Add(new Vector2(x, y), createdItemEnemy.GetComponent<GuardEnemy>());
                        map.enemy_map[new Vector2(x, y)].tile_position = new Vector2(x, y);
                        map.enemy_map[new Vector2(x, y)].tile_value = (TileValues)tile_value;
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
                    if (createdItemEnemy.GetComponent<Gate>() != null)
                    {
                        createdItemEnemy.GetComponent<SpriteRenderer>().sortingOrder = Constants.MapHeight - y;
                    }
                    else
                    {
                        createdItemEnemy.GetComponent<SpriteRenderer>().sortingOrder = Constants.MapHeight - y-1;
                    }
                }

                ground_trigger = !ground_trigger;
            }
        }

        // Presureplates/Gates
        if (ResourceLoader.HasLevelTextFile(level_name + "_pp"))
        {
            string[] ppContent = ResourceLoader.GetLevelTextFile(level_name + "_pp").text.Split('\n');
            foreach(string s in ppContent)
            {
                if (s != string.Empty)
                {
                    string[] split = s.Split(null);
                    int x1, x2, y1, y2;
                    // Get the coordinates from the line as integers
                    int.TryParse(split[0], out x1);
                    int.TryParse(split[1], out y1);
                    int.TryParse(split[3], out x2);
                    int.TryParse(split[4], out y2);
                    // Update the presure plate to know which gate to open!
                    map.pp_map[new Vector2(x1, y1)].gate_position = new Vector2(x2, y2);
                }
            }

        }

        return map;
    }

    private GameObject CreateGround(int x, int y)
    {
        Sprite sprite = Ground;
        int value = TiledImporter.CurrentGroundLayer[y, x];
        switch (value)
        {
            case (int)TileValues.GROUND:
                sprite = Ground;
                break;
            case (int)TileValues.GROUND_CRACKED1:
                sprite = GroundCracked1;
                break;
            case (int)TileValues.GROUND_CRACKED2:
                sprite = GroundCracked2;
                break;
            case (int)TileValues.GROUND_CRACKED3:
                sprite = GroundCracked3;
                break;
            case (int)TileValues.GROUND_CRACKED4:
                sprite = GroundCracked4;
                break;
            case (int)TileValues.GROUND_CRACKED5:
                sprite = GroundCracked5;
                break;
            case (int)TileValues.GROUND_MOSSY1:
                sprite = GroundMossy1;
                break;
            case (int)TileValues.GROUND_MOSSY2:
                sprite = GroundMossy2;
                break;
            case (int)TileValues.GROUND_MOSSY3:
                sprite = GroundMossy3;
                break;
            case (int)TileValues.GROUND_MOSSY4:
                sprite = GroundMossy4;
                break;
            case (int)TileValues.GROUND_MOSSY5:
                sprite = GroundMossy5;
                break;
            case (int)TileValues.PP_GROUND_RL:
                sprite = PPGroundRL;
                break;
            case (int)TileValues.PP_GROUND_LD:
                sprite = PPGroundLD;
                break;
            case (int)TileValues.PP_GROUND_UR:
                sprite = PPGroundUR;
                break;
            case (int)TileValues.PP_GROUND_UL:
                sprite = PPGroundUL;
                break;
            case (int)TileValues.PP_GROUND_LR:
                sprite = PPGroundLR;
                break;
            case (int)TileValues.PP_GROUND_UD:
                sprite = PPGroundUD;
                break;
        }

        GameObject createdGround = new GameObject();
        createdGround.name = "ground(" + x.ToString() + ", " + y.ToString() + ")";
        createdGround.AddComponent<SpriteRenderer>();
        createdGround.GetComponent<SpriteRenderer>().sprite = sprite;
        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
        createdGround.transform.SetParent(map_holder.transform);

        return createdGround;
    }

    public static bool IsWalkable(int tile_value)
    {
        for(int i = (int)TileValues.GROUND_CRACKED1; i <= (int)TileValues.GROUND_CRACKED5; i++)
        {
            if (tile_value == i)
                return true;
        }
        for(int i = (int)TileValues.PP_GROUND_RL; i <= (int)TileValues.PP_GROUND_UD; i++)
        {
            if (tile_value == i)
                return true;
        }
        for(int i = (int)TileValues.GROUND_MOSSY1; i <= (int)TileValues.GROUND_MOSSY5; i++)
        {
            if(tile_value == i)
                return true;
        }
        if(tile_value == (int)TileValues.GROUND)
        {
            return true;
        }

        return false;
    }

    public static bool IsFallSpike(int tile_value)
    {
        switch (tile_value)
        {
            case (int)TileValues.FALL_SPIKE_CONNECTOR_VERTICAL:
            case (int)TileValues.FALL_SPIKE_START_VERTICAL:
            case (int)TileValues.FALL_SPIKE_ENDER_VERTICAL:
            case (int)TileValues.FALL_SPIKE:
                return true;
            default:
                return false;
        }
    }

    public static bool IsItem(int tile_value)
    {
        if(tile_value == (int)TileValues.RED_KEY || tile_value == (int)TileValues.BLUE_KEY || tile_value == (int)TileValues.WEAPON || tile_value == (int)TileValues.SHIELD || tile_value == (int)TileValues.HEALTH)
        {
            return true;
        }

        return false;
    }

    public static bool IsEnemy(int tile_value)
    {
        switch (tile_value)
        {
            case (int)TileValues.GUARD_LEFT:
            case (int)TileValues.GUARD_UP:
            case (int)TileValues.DESKGOBLIN:
                return true;
        }
        return false;
    }

    public static bool IsGate(int tile_value)
    {
        switch (tile_value)
        {
            case (int)TileValues.GATE_FORWARD_BLUE:
            case (int)TileValues.GATE_FORWARD_RED:
            case (int)TileValues.GATE_SIDE_BLUE:
            case (int)TileValues.GATE_SIDE_RED:
            case (int)TileValues.PP_GATE_FORWARD:
            case (int)TileValues.PP_GATE_SIDE:
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
        if (IsWalkable(tile_value1) && (IsEnemy(tile_value2) || IsItem(tile_value2) || tile_value2 == (int)TileValues.PRESURE_PLATE || tile_value2 == (int)TileValues.BLOCK || IsFallSpike(tile_value2) || tile_value2 == (int)TileValues.SPIKE || IsGate(tile_value2)))
        {
                return true;
        }
        return false;
    }
}