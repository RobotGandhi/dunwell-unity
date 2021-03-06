﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : TouchListener
{
    List<Vector3> direction_to_vector = new List<Vector3>();

    public Sprite ground_sprite;
    public GameObject food_particle_prefab;

    GameMaster g_master;
    TouchSystem t_system;
    SoundEffects sfx;
    [System.NonSerialized]
    public PlayerCombat player_combat;
    [System.NonSerialized]
    public PlayerAnimation player_animation;

    [System.NonSerialized]
    public Vector3 tile_position = new Vector3(1, 1, 0); // @ Why tf is this a vector3?

    Enums.PlayerStates player_state;
    Enums.PlayerMoveDirection move_direction;

    SpriteRenderer spre;

    public Transform current_item;
    private float weapon_offset;
    private float shield_offset;
    private float health_offset;
    private float key_offset;

    public int HP = 3;
    public Image[] hp_objects;

    public static float fall_speed = 25;

    bool walk_flag = false;
    public bool die_flag = false;
    bool walk_finger_down = false;

    bool block_input = false; // If true no input should be taken for the player to perform actions!

    void Awake()
    {
        g_master = FindObjectOfType<GameMaster>();
        sfx = FindObjectOfType<SoundEffects>();
        player_animation = GetComponent<PlayerAnimation>();

        spre = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Direction to vector list
        direction_to_vector.Add(new Vector3(0, 1));  // up
        direction_to_vector.Add(new Vector3(1, 0));  // right
        direction_to_vector.Add(new Vector3(0, -1)); // down
        direction_to_vector.Add(new Vector3(-1, 0)); // left

        t_system = FindObjectOfType<TouchSystem>();
        t_system.AddTouchListener(this);

        // Item
        current_item = null;
        weapon_offset = MapManager.GroundTileSize * 0.35f;
        shield_offset = MapManager.GroundTileSize * 0.55f;
        health_offset = MapManager.GroundTileSize * 0.5f;
        key_offset = MapManager.GroundTileSize * 0.3f;
    }

    void Update()
    {
        // Moving the player if he's supposed to be "moving"
        if (player_state == Enums.PlayerStates.MOVING)
        {
            // move towards desired tile position
            if (transform.position != (tile_position * MapManager.GroundTileSize))
            {
                transform.position = Vector3.MoveTowards(transform.position, (tile_position * MapManager.GroundTileSize), Constants.ObjectMoveSpeed * Time.deltaTime);
            }
            else
            {
                // We reached desired tile position
                ReachedNewTile();
            }
        }

        if(player_state == Enums.PlayerStates.IDLE)
        {
            if (die_flag)
            {
                die_flag = false;
                Die();
            }
        }

        /* Item */
        if (current_item != null)
        {
            // Layer
            current_item.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;

            // Position
            switch (current_item.GetComponent<Item>().item_type)
            {
                case Item.ItemType.WEAPON:
                    current_item.position = transform.position + Vector3.up * weapon_offset;
                    break;
                case Item.ItemType.SHIELD:
                    current_item.position = transform.position + Vector3.up * shield_offset;
                    break;
                case Item.ItemType.HEALTH:
                    current_item.position = transform.position + Vector3.up * health_offset;
                    break;
                case Item.ItemType.BLUE_KEY:
                case Item.ItemType.RED_KEY:
                    current_item.position = transform.position + Vector3.up * key_offset;
                    break;
            }
        }

        /* HP */
        HPEnableLogic();

        if (Input.touchCount == 0)
            walk_finger_down = false;
        if (walk_finger_down)
        {
            if(player_state == Enums.PlayerStates.IDLE)
            {
                MovePlayer(move_direction);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ConsumeFood();
        }

        #region WASD
        if (block_input)
            return;
        if (player_state == Enums.PlayerStates.IDLE)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                MovePlayer(Enums.PlayerMoveDirection.RIGHT);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                MovePlayer(Enums.PlayerMoveDirection.LEFT);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MovePlayer(Enums.PlayerMoveDirection.DOWN);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                MovePlayer(Enums.PlayerMoveDirection.UP);
            }
        }
        #endregion
    }

    // Called from update when player reaches desired transform position
    private void ReachedNewTile()
    {
        if (die_flag)
        {
            die_flag = false;
            Die();
        }
        else if (player_state == Enums.PlayerStates.MOVING)
        {
            SetPlayerState(Enums.PlayerStates.IDLE);
        }
    }

    void SetPlayerState(Enums.PlayerStates state)
    {
        player_state = state;

        if (player_state == Enums.PlayerStates.IDLE)
        {
            // Do idle stuff
            player_animation.StartIdle();
        }
        else if (player_state == Enums.PlayerStates.MOVING)
        {
            // Do move stuff
            player_animation.StartMoving(move_direction);
        }
    }

    void MovePlayer(Enums.PlayerMoveDirection direction, string walk_sfx = "default")
    {
        // Get the new tile value and tile position
        Vector2 new_tile_position = tile_position + direction_to_vector[(int)direction];
        int new_tile_value = g_master.current_map.tile_map[(int)new_tile_position.y, (int)new_tile_position.x];

        if (MapManager.IsWalkable(new_tile_value))
        {
            DoMovePlayer(direction, new_tile_position);
            walk_finger_down = true;
        }
        else if (MapManager.IsItem(new_tile_value))
        {
            GameObject temp_item = g_master.current_map.item_map[new Vector2(new_tile_position.x, new_tile_position.y)];

            // Remove item from tile map and make it into a ground tile
            g_master.current_map.tile_map[(int)new_tile_position.y, (int)new_tile_position.x] = (int)MapManager.TileValues.GROUND;

            // Destroy current item
            RemoveCurrentItem();
            current_item = temp_item.transform;
            player_animation.ItemChange();
            // SFX
            switch (current_item.GetComponent<Item>().item_type)
            {
                case Item.ItemType.HEALTH:
                    sfx.PlaySFX("food_pickup");
                    break;
                case Item.ItemType.WEAPON:
                    sfx.PlaySFX("sword_pickup");
                    break;
                case Item.ItemType.SHIELD:
                    sfx.PlaySFX("armor_pickup");
                    break;
                case Item.ItemType.BLUE_KEY:
                case Item.ItemType.RED_KEY:
                    sfx.PlaySFX("key_pickup");
                    break;
            }

            /* Move the player */
            DoMovePlayer(direction, new_tile_position);

        }
        else if (new_tile_value == (int)MapManager.TileValues.GOAL1)
        {
            DoMovePlayer(direction, new_tile_position);
            StartCoroutine("OutroCoroutine");
        }
        else if (MapManager.IsEnemy(new_tile_value))
        {
            if (g_master.current_map.enemy_map.ContainsKey(new_tile_position))
            {
                // Call on player_combat to perform the combat for us
                player_combat.EngageEnemy(g_master.current_map.enemy_map[new_tile_position], new_tile_position);

                FindObjectOfType<EventSystem>().PlayerPerformedEvent();

                player_animation.SetMoveDirection(direction);
            }
            else
            {
                DoMovePlayer(direction, new_tile_position);
            }
        }
        else if (new_tile_value == (int)MapManager.TileValues.SPIKE)
        {
            DoMovePlayer(direction, new_tile_position);
        }
        else if (MapManager.IsGate(new_tile_value))
        {
            Gate _gate = g_master.current_map.gate_map[new_tile_position];
            // Check if gate is already open
            if (_gate.IsOpen())
            {
                DoMovePlayer(direction, new_tile_position);
            }
            else
            {
                // Open gate!
                // Make sure we have correct key item
                if(current_item != null)
                {
                    if ((current_item.GetComponent<Item>().item_type == Item.ItemType.BLUE_KEY && _gate.door_color == "blue") ||
                        current_item.GetComponent<Item>().item_type == Item.ItemType.RED_KEY && _gate.door_color == "red")
                    {
                        _gate.Open();
                        RemoveCurrentItem(true);
                    }

                }
            }
        }
        else if (MapManager.IsFallSpike(new_tile_value))
        {
            DoMovePlayer(direction, new_tile_position);
            die_flag = true;
        }
        else if(new_tile_value == (int)MapManager.TileValues.PRESURE_PLATE)
        {
            DoMovePlayer(direction, new_tile_position);
            g_master.current_map.pp_map[new Vector2((int)tile_position.x, (int)tile_position.y)].Enable();
        }
    }

    public void Die()
    {
        RemoveCurrentItem();
        sfx.PlaySFX("player_die");
        player_animation.Die();
        SetPlayerState(Enums.PlayerStates.DEAD);
        g_master.PlayerDie();
    }

    private void DoMovePlayer(Enums.PlayerMoveDirection direction, Vector2 new_tile_position, string walk_sfx = "default")
    {
        
        // Set move direction
        move_direction = direction;
        // Set to move state
        SetPlayerState(Enums.PlayerStates.MOVING);
        // Set players new tile position
        tile_position = new_tile_position;
        
        // After the players tile position has been set we message the others
        FindObjectOfType<EventSystem>().PlayerPerformedEvent();

        // Spre
        spre.sortingOrder = (Constants.MapHeight - (int)new_tile_position.y);


        // SFX
        if (walk_sfx == "default")
        {
            if (walk_flag) sfx.PlaySFX("footstep1");
            if (!walk_flag) sfx.PlaySFX("footstep2");
        }
        else if (walk_sfx != "none")
        {
            sfx.PlaySFX(walk_sfx);
        }
        walk_flag = !walk_flag;
    }

    public override void HorizontalSwipe(int direction)
    {
        if (block_input)
            return;

        if (player_state == Enums.PlayerStates.IDLE)
        {
            if (direction > 0)
            {
                MovePlayer(Enums.PlayerMoveDirection.RIGHT);
            }
            else
            {
                MovePlayer(Enums.PlayerMoveDirection.LEFT);
            }
        }
    }

    public override void VerticalSwipe(int direction)
    {
        if (block_input)
            return;

        if (player_state == Enums.PlayerStates.IDLE)
        {
            if (direction > 0)
            {
                MovePlayer(Enums.PlayerMoveDirection.UP);
            }
            else
            {
                MovePlayer(Enums.PlayerMoveDirection.DOWN);
            }
        }
    }

    public override void FingerUp()
    {
        walk_finger_down = false;
    }

    public override void DoubleTap()
    {
        if (block_input)
            return;

        if (current_item != null)
        {
            if (current_item.GetComponent<Item>().item_type == Item.ItemType.HEALTH)
            {
                ConsumeFood();
            }
        }
    }

    public void ConsumeFood()
    {
        if (HP < 3)
            HP++;
        else
            return;
        sfx.PlaySFX("eating_health_up");
        
        GameObject t = Instantiate(food_particle_prefab, transform.position, Quaternion.identity) as GameObject;
        t.transform.Rotate(new Vector3(-90, 0, 0));

        RemoveCurrentItem();

        FindObjectOfType<EventSystem>().PlayerPerformedEvent();
    }

    private void HPEnableLogic()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i >= HP)
            {
                hp_objects[i].color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                hp_objects[i].color = Color.white;
            }
        }
    }

    public void NewLevel(Vector2 tile_position)
    {
        // Set position
        this.tile_position = tile_position;
        transform.position = new Vector3(tile_position.x, tile_position.y) * MapManager.GroundTileSize;

        // Reset some stuff
        spre.color = Color.white;
        spre.sortingLayerName = "player_items_enemies";
        spre.sortingOrder = (int)tile_position.y;
        if(current_item != null)
            RemoveCurrentItem();
        HP = 3;

        SetPlayerState(Enums.PlayerStates.IDLE);
    }

    private IEnumerator HideSwordForAnimation()
    {
        current_item.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.333f / 2.0f);
        current_item.gameObject.SetActive(true);
    }

    private IEnumerator OutroCoroutine()
    {
        while (player_state != Enums.PlayerStates.IDLE)
        {
            yield return new WaitForEndOfFrame();
        }

        sfx.PlaySFX("falling");
        player_state = Enums.PlayerStates.OUTRO;

        spre.sortingLayerName = "room_tiles";
        spre.sortingOrder = -1;
        g_master.current_map.goal.GetComponent<SpriteRenderer>().sortingOrder = -2;

        while (spre.color.a > 0)
        {
            spre.color = Vector4.MoveTowards(spre.color, new Vector4(1, 1, 1, 0), 1.0f * Time.deltaTime);
            transform.position += Vector3.down * fall_speed * Time.deltaTime;
            if (current_item != null) current_item.GetComponent<SpriteRenderer>().color = spre.color;
            yield return new WaitForEndOfFrame();
        }

        RemoveCurrentItem();

        g_master.NewMap();
    }

    public override void FingerDown(int index, Vector2 pos)
    {
        if (block_input)
            return;

        Vector2 rel_pos = new Vector2(pos.x / Screen.width, pos.y / Screen.height);
        if(rel_pos.y >= 0.6 || rel_pos.y <= 0.4)
        {
            if(rel_pos.y >= 0.7)
            {
                MovePlayer(Enums.PlayerMoveDirection.UP);
            }
            else if(rel_pos.y <= 0.3)
            {
                MovePlayer(Enums.PlayerMoveDirection.DOWN);
            }
        }
        else
        {
            if(rel_pos.x >= 0.5)
            {
                MovePlayer(Enums.PlayerMoveDirection.RIGHT);
            }
            else
            {
                MovePlayer(Enums.PlayerMoveDirection.LEFT);
            }
        }
    }

    public void RemoveCurrentItem(bool destroy_key = false)
    {
        if(current_item != null)
        {
            if ((current_item.GetComponent<Item>().item_type == Item.ItemType.BLUE_KEY || current_item.GetComponent<Item>().item_type == Item.ItemType.RED_KEY) && !destroy_key)
            {
                Vector2 item_tile_pos = current_item.GetComponent<Item>().spawn_tile_position;
                int item_value = (int)current_item.GetComponent<Item>().item_type;

                GameObject last_item = current_item.gameObject;
                current_item = null;
                
                g_master.current_map.tile_map[(int)item_tile_pos.y, (int)item_tile_pos.x] = item_value;
                last_item.GetComponent<Item>().ResetPosition();

            }
            else
            {
                g_master.current_map.item_map.Remove(current_item.GetComponent<Item>().spawn_tile_position);
                Destroy(current_item.gameObject);
                current_item = null;
            }
        }
        player_animation.ItemChange();
    }

    public void TakeSpikeHit()
    {
        bool hadShield = false;
        // Do we have a shield?
        if (current_item != null)
        {
            if (current_item.GetComponent<Item>().item_type == Item.ItemType.SHIELD)
            {
                Animator _spikeAnimator = g_master.current_map.spike_map[tile_position].GetComponent<Animator>();
                _spikeAnimator.enabled = true;
                _spikeAnimator.SetTrigger("action");
                player_combat.RemoveShield();
                Camera.main.GetComponent<CameraShake>().DoShake(Constants.LightCamShake);
                hadShield = true;
            }
        }
        if (!hadShield)
        {
            Animator _spikeAnimator = g_master.current_map.spike_map[tile_position].GetComponent<Animator>();
            _spikeAnimator.enabled = true;
            _spikeAnimator.SetTrigger("action");
            die_flag = true;
        }
    }

    public void BlockInput()
    {
        block_input = true;
    }

    public void UnblockInput()
    {
        block_input = false;
    }

}
