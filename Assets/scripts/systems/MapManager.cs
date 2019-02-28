﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //public Sprite ground_sprite, ground_sprite2, left_wall_sprite, right_wall_sprite, top_wall_sprite, pit_sprite, bot_left_wall_sprite, bot_right_wall_sprite, goal_sprite1, goal_sprite2;
    public Sprite weapon_sprite, shield_sprite, health_sprite;

    public static float GroundTileSize;

    public GameObject skeleton_prefab;

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

        NONE = -1
    };

    static int GR = (int)TileValues.GROUND;
    static int LW = (int)TileValues.LEFT_WALL;
    static int RW = (int)TileValues.RIGHT_WALL;
    static int TW = (int)TileValues.TOP_WALL;
    static int PT = (int)TileValues.PIT;
    static int NN = (int)TileValues.NONE;
    static int BL = (int)TileValues.BOT_LEFT_WALL;
    static int BR = (int)TileValues.BOT_RIGHT_WALL;
    static int WI = (int)TileValues.WEAPON;
    static int SI = (int)TileValues.SHIELD;
    static int HI = (int)TileValues.HEALTH;
    static int SK = (int)TileValues.SKELETON;
    static int GO = (int)TileValues.GOAL;

    bool ground_trigger = false;

    static int[,] map1 =
    {
        { NN, PT, PT, PT, PT, NN },
        { BL, GR, GR, GR, GR, BR },
        { BL, GR, GR, GR, GR, BR },
        { BL, GR, GR, GR, GR, BR },
        { LW, GR, GR, GR, GR, RW },
        { LW, GR, GR, GR, GR, RW },
        { LW, GR, GR, GR, SK, RW },
        { LW, GR, GR, GR, GO, RW },
        { LW, TW, TW, TW, TW, RW }
    };

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
                switch (tile_value)
                {
                    case (int)TileValues.GROUND:
                        createdGround = new GameObject();
                        string name = "ground(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.name = name;
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = ground_trigger ? ResourceLoader.GetSprite("floor0") : ResourceLoader.GetSprite("floor1");
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        createdGround.GetComponent<SpriteRenderer>().sortingOrder = y;
                        break;
                    case (int)TileValues.LEFT_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("wallLeft");
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        createdGround.GetComponent<SpriteRenderer>().sortingOrder = y;
                        break;
                    case (int)TileValues.RIGHT_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("wallRight");
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        createdGround.GetComponent<SpriteRenderer>().sortingOrder = y;
                        break;
                    case (int)TileValues.TOP_WALL:
                        createdGround = new GameObject();
                        createdGround.name = "top_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        createdGround.AddComponent<SpriteRenderer>();
                        createdGround.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("wallTop");
                        createdGround.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        createdGround.transform.SetParent(map_holder.transform);
                        createdGround.GetComponent<SpriteRenderer>().sortingOrder = y;
                        break;
                    case (int)TileValues.PIT:
                        GameObject pit = new GameObject();
                        pit.name = "pit(" + x.ToString() + ", " + y.ToString() + ")";
                        pit.AddComponent<SpriteRenderer>();
                        pit.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("pit");
                        pit.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        pit.transform.SetParent(map_holder.transform);
                        pit.GetComponent<SpriteRenderer>().sortingOrder = y;
                        break;
                    case (int)TileValues.BOT_LEFT_WALL:
                        GameObject bot_left_wall = new GameObject();
                        bot_left_wall.name = "bot_left_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        bot_left_wall.AddComponent<SpriteRenderer>();
                        bot_left_wall.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("bottomLeftWall");
                        bot_left_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        bot_left_wall.transform.SetParent(map_holder.transform);
                        bot_left_wall.GetComponent<SpriteRenderer>().sortingOrder = y;
                        break;
                    case (int)TileValues.BOT_RIGHT_WALL:
                        GameObject bot_right_wall = new GameObject();
                        bot_right_wall.name = "bot_right_wall(" + x.ToString() + ", " + y.ToString() + ")";
                        bot_right_wall.AddComponent<SpriteRenderer>();
                        bot_right_wall.GetComponent<SpriteRenderer>().sprite = ResourceLoader.GetSprite("bottomRightWall");
                        bot_right_wall.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        bot_right_wall.transform.SetParent(map_holder.transform);
                        bot_right_wall.GetComponent<SpriteRenderer>().sortingOrder = y;
                        break;
                    case (int)TileValues.WEAPON:
                        // First spawn ground
                        GameObject weapon_ground = new GameObject();
                        weapon_ground.name = "ground(" + x.ToString() + ", " + y.ToString() + ")";
                        weapon_ground.AddComponent<SpriteRenderer>();
                        weapon_ground.GetComponent<SpriteRenderer>().sprite = ground_trigger ? ResourceLoader.GetSprite("floor0") : ResourceLoader.GetSprite("floor1");
                        weapon_ground.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        weapon_ground.transform.SetParent(map_holder.transform);
                        weapon_ground.GetComponent<SpriteRenderer>().sortingOrder = y;
                        // Spawn weapon
                        GameObject weapon = new GameObject();
                        weapon.name = "weapon";
                        weapon.AddComponent<SpriteRenderer>();
                        weapon.GetComponent<SpriteRenderer>().sprite = weapon_sprite;
                        weapon.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        weapon.transform.SetParent(map_holder.transform);
                        weapon.GetComponent<SpriteRenderer>().sortingOrder = y + 1;

                        // Item stuff
                        weapon.AddComponent<Item>();
                        weapon.GetComponent<Item>().item_type = Item.ItemType.WEAPON;
                        map.item_map.Add(new Vector2(x, y), weapon);
                        break;
                    case (int)TileValues.SHIELD:
                        // First spawn ground
                        GameObject armor_ground = new GameObject();
                        armor_ground.name = "ground(" + x.ToString() + ", " + y.ToString() + ")";
                        armor_ground.AddComponent<SpriteRenderer>();
                        armor_ground.GetComponent<SpriteRenderer>().sprite = ground_trigger ? ResourceLoader.GetSprite("floor0") : ResourceLoader.GetSprite("floor1");
                        armor_ground.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        armor_ground.transform.SetParent(map_holder.transform);
                        armor_ground.GetComponent<SpriteRenderer>().sortingOrder = y;
                        // Spawn weapon
                        GameObject shield = new GameObject();
                        shield.name = "shield";
                        shield.AddComponent<SpriteRenderer>();
                        shield.GetComponent<SpriteRenderer>().sprite = shield_sprite;
                        shield.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        shield.transform.SetParent(map_holder.transform);
                        shield.GetComponent<SpriteRenderer>().sortingOrder = y + 1;

                        // Item stuff
                        shield.AddComponent<Item>();
                        shield.GetComponent<Item>().item_type = Item.ItemType.SHIELD;
                        map.item_map.Add(new Vector2(x, y), shield);
                        break;
                    case (int)TileValues.HEALTH:                        // First spawn ground
                        GameObject health_ground = new GameObject();
                        health_ground.name = "ground(" + x.ToString() + ", " + y.ToString() + ")";
                        health_ground.AddComponent<SpriteRenderer>();
                        health_ground.GetComponent<SpriteRenderer>().sprite = ground_trigger ? ResourceLoader.GetSprite("floor0") : ResourceLoader.GetSprite("floor1");
                        health_ground.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        health_ground.transform.SetParent(map_holder.transform);
                        health_ground.GetComponent<SpriteRenderer>().sortingOrder = y;
                        // Spawn weapon
                        GameObject health = new GameObject();
                        health.name = "health";
                        health.AddComponent<SpriteRenderer>();
                        health.GetComponent<SpriteRenderer>().sprite = health_sprite;
                        health.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        health.transform.SetParent(map_holder.transform);
                        health.GetComponent<SpriteRenderer>().sortingOrder = y + 1;

                        // Item stuff
                        health.AddComponent<Item>();
                        health.GetComponent<Item>().item_type = Item.ItemType.HEALTH;
                        map.item_map.Add(new Vector2(x, y), health);
                        break;
                    case (int)TileValues.SKELETON:
                        GameObject skel_ground = new GameObject();
                        skel_ground.name = "ground(" + x.ToString() + ", " + y.ToString() + ")";
                        skel_ground.AddComponent<SpriteRenderer>();
                        skel_ground.GetComponent<SpriteRenderer>().sprite = ground_trigger ? ResourceLoader.GetSprite("floor0") : ResourceLoader.GetSprite("floor1");
                        skel_ground.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize, 0);
                        skel_ground.transform.SetParent(map_holder.transform);
                        // Spawn skeleton
                        GameObject skeleton = Instantiate(skeleton_prefab);
                        skeleton.transform.position = new Vector3(x * GroundTileSize, y * GroundTileSize);
                        skeleton.GetComponent<SpriteRenderer>().sortingOrder = y;
                        skeleton.transform.SetParent(map_holder.transform);
                        map.enemy_map.Add(new Vector2(x, y), skeleton.GetComponent<Enemy>());
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
                }

                ground_trigger = !ground_trigger;
            }
        }

        return map;
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

    /*
     * Used in the importer to check if tile_value1 should be replaced by tile_value2
    */
    public static bool ShouldReplace(int tile_value1, int tile_value2)
    {
        // If tile_value2 is enemy or item and tile_value1 is walkable then replace 
        if(IsWalkable(tile_value1) && (IsEnemy(tile_value2) || IsItem(tile_value2)))
        {
            return true;
        }
        return false;
    }

}