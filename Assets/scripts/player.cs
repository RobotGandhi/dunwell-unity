using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : TouchListener
{
    List<Vector3> direction_to_vector = new List<Vector3>();

    public Sprite ground_sprite;
    public GameObject enemy_remains_prefab;

    MapManager m_manager;
    GameMaster g_master;
    TouchSystem t_system;
    SoundEffects sfx;

    private Vector3 tile_position = new Vector3(1, 1, 0);
    const float move_speed = 7.5f;

    Enums.PlayerStates player_state;
    Enums.PlayerMoveDirection move_direction;
    Enums.PlayerMoveDirection ice_slide_direction;

    Animator anim_controller;
    SpriteRenderer spre;

    bool idle_trigger = false;
    bool move_finger_down = false;

    public Transform current_item;
    private float weapon_offset;
    private float shield_offset;
    private float health_offset;

    public int HP = 3;
    public Image[] hp_objects;

    public static float fall_speed = 25;

    bool walk_flag = false;

    void Start()
    {
        // Direction to vector list
        direction_to_vector.Add(new Vector3(0, 1));  // up
        direction_to_vector.Add(new Vector3(1, 0));  // right
        direction_to_vector.Add(new Vector3(0, -1)); // down
        direction_to_vector.Add(new Vector3(-1, 0)); // left

        t_system = FindObjectOfType<TouchSystem>(); // There should only be ONE!
        t_system.AddTouchListener(this);

        g_master = FindObjectOfType<GameMaster>();

        m_manager = FindObjectOfType<MapManager>();

        sfx = FindObjectOfType<SoundEffects>();

        // Get the animator
        anim_controller = GetComponent<Animator>();
        anim_controller.SetBool("idle", true);
        anim_controller.SetBool("holding_item", false);
        anim_controller.SetBool("moving", false);

        spre = GetComponent<SpriteRenderer>();

        // Item
        current_item = null;
        weapon_offset = MapManager.GroundTileSize * 0.35f;
        shield_offset = MapManager.GroundTileSize * 0.55f;
        health_offset = MapManager.GroundTileSize * 0.5f;
    }

    void Update()
    {
        if (player_state == Enums.PlayerStates.MOVING)
        {
            if (transform.position != tile_position * MapManager.GroundTileSize)
            {
                transform.position = Vector3.MoveTowards(transform.position, tile_position * MapManager.GroundTileSize, move_speed * Time.deltaTime);
            }
            else
            {
                ReachedNewTile();
            }
        }

        // Check if a finger is down
        if(Input.touchCount > 0)
        {
            if (idle_trigger && move_finger_down)
            {
                MovePlayer(move_direction);
            }
        }

        idle_trigger = false;

        /* Item */
        if(current_item != null)
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
            }
        }

        /* HP */
        HPEnableLogic();

        #region WASD
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
        // If we're on ice we dont want to simply go to back to idle, we want to slip n' slide
        if (player_state == Enums.PlayerStates.MOVING)
        {
            SetPlayerState(Enums.PlayerStates.IDLE);
        }
    }

    void SetPlayerState(Enums.PlayerStates state)
    {
        player_state = state;

        if(player_state == Enums.PlayerStates.IDLE)
        {
            // Do idle stuff
            anim_controller.SetBool("moving", false);
            anim_controller.SetBool("idle", true);
            idle_trigger = true;
        }
        else if(player_state == Enums.PlayerStates.MOVING)
        {
            // Do move stuff
            anim_controller.SetTrigger("move_trigger");
            anim_controller.SetBool("moving", true);
            anim_controller.SetBool("idle", false);
            anim_controller.SetInteger("move_direction", (int)move_direction);
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
        }
        else if (MapManager.IsItem(new_tile_value))
        {
            GameObject temp = g_master.current_map.item_map[new Vector2(new_tile_position.x, new_tile_position.y)];

            if (temp.GetComponent<Item>().item_state == Item.ItemState.ON_MAP)
            {
                // Destroy current item
                if (current_item != null)
                {
                    current_item.GetComponent<Item>().SetState(Item.ItemState.DISCARDED_FROM_MAP);
                }
                current_item = temp.transform;
                current_item.GetComponent<Item>().SetState(Item.ItemState.PICKED_UP);
                anim_controller.SetBool("holding_item", true);
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
                }
            }

            /* Move the player */
            DoMovePlayer(direction, new_tile_position);
            
        }
        else if (MapManager.IsEnemy(new_tile_value))
        {
            if (g_master.current_map.enemy_map.ContainsKey(new_tile_position))
            {
                Combat.CombatResult result = Combat.PerformCombat(this, g_master.current_map.enemy_map[new_tile_position].GetComponent<Enemy>());

                anim_controller.SetInteger("move_direction", (int)direction);

                g_master.Step();

                switch (result)
                {
                    case Combat.CombatResult.CLASH:
                        sfx.PlaySFX("enemy_hurt");
                        break;
                    case Combat.CombatResult.PLAYER_DIED:
                        Die();
                        break;
                    case Combat.CombatResult.ENEMY_DIED:
                        // Spawn enemy remains
                        GameObject temp = (Instantiate(enemy_remains_prefab, g_master.current_map.enemy_map[new_tile_position].transform.position, Quaternion.identity)) as GameObject;
                        temp.GetComponent<SpriteRenderer>().sortingOrder++;
                        temp.transform.SetParent(m_manager.map_holder.transform);
                        // Remove from world
                        Destroy(g_master.current_map.enemy_map[new_tile_position].gameObject);
                        g_master.current_map.enemy_map.Remove(new_tile_position);
                        // Play SFX
                        sfx.PlaySFX("enemy_dead_bones");
                        break;
                    case Combat.CombatResult.SHIELD_DEFEND:
                        anim_controller.SetBool("holding_item", false);
                        // SFX
                        sfx.PlaySFX("enemy_hurt");
                        sfx.PlaySFX("armor_break");
                        // Remove shield
                        current_item.GetComponent<Item>().SetState(Item.ItemState.DISCARDED_FROM_MAP);
                        current_item = null;
                        break;
                }

                // Animation
                if (result != Combat.CombatResult.PLAYER_DIED)
                {
                    if (current_item != null)
                    {
                        if (current_item.GetComponent<Item>().item_type == Item.ItemType.WEAPON)
                        {
                            StartCoroutine("HideSwordForAnimation");
                            anim_controller.SetTrigger("attack_sword");
                        }
                        else if (current_item.GetComponent<Item>().item_type == Item.ItemType.HEALTH)
                        {
                            anim_controller.SetTrigger("attack_item");
                        }
                        else
                        {
                            anim_controller.SetTrigger("attack");
                        }
                    }
                    else
                    {
                        anim_controller.SetTrigger("attack");
                    }
                }
            }
            else
            {
                DoMovePlayer(direction, new_tile_position);
            }
        }
        else if(new_tile_value == (int)MapManager.TileValues.GOAL)
        {
            DoMovePlayer(direction, new_tile_position);
            StartCoroutine("OutroCoroutine");
        }
    }

    public void Die()
    {
        if(current_item != null)
        {
            current_item.GetComponent<Item>().SetState(Item.ItemState.DISCARDED_FROM_MAP);
            current_item = null;
        }
        sfx.PlaySFX("player_die");
        anim_controller.SetTrigger("die");
        SetPlayerState(Enums.PlayerStates.DEAD);
        g_master.PlayerDie();
    }

    private void DoMovePlayer(Enums.PlayerMoveDirection direction, Vector2 new_tile_position, string walk_sfx = "default")
    {
        /* Move the player */
        g_master.Step();
        // Set move direction
        move_direction = direction;
        // Set to move state
        SetPlayerState(Enums.PlayerStates.MOVING);
        // Set players new tile position
        tile_position = new_tile_position;
        // Assume the movefinger down
        move_finger_down = true;
        // Spre
        spre.sortingOrder = (int)new_tile_position.y+2;
        // SFX
        if (walk_sfx == "default")
        {
            if (walk_flag) sfx.PlaySFX("footstep1");
            if (!walk_flag) sfx.PlaySFX("footstep2");
        }
        else if(walk_sfx != "none")
        {
            sfx.PlaySFX(walk_sfx);
        }
        walk_flag = !walk_flag;
    }

    public override void HorizontalSwipe(int direction)
    {
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
        if(player_state == Enums.PlayerStates.MOVING)
        {
            move_finger_down = false;
        }
    }

    public override void DoubleTap()
    {
        if(current_item != null)
        {
            if(current_item.GetComponent<Item>().item_type == Item.ItemType.HEALTH)
            {
                HP++;
                current_item.GetComponent<Item>().SetState(Item.ItemState.DISCARDED_FROM_MAP);
                current_item = null;
                anim_controller.SetBool("holding_item", false);
                sfx.PlaySFX("eating_health_up");

                g_master.Step();
            }
        }
    }

    private void HPEnableLogic()
    {
        for(int i = 0; i < 3; i++)
        {
            if(i >= HP)
            {
                hp_objects[i].color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                hp_objects[i].color = Color.white;
            }
        }
    }

    public void PlayIntroAt(Vector2 tile_position)
    {
        SetPlayerState(Enums.PlayerStates.INTRO);
        this.tile_position = tile_position;
        // Position for intro
        transform.position = new Vector3(tile_position.x, tile_position.y + 6) * ground_sprite.bounds.size.x;
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().sortingOrder = (int)tile_position.y+6;

        StartCoroutine("IntroCoroutine");
    }

    private IEnumerator HideSwordForAnimation()
    {
        current_item.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.333f/2.0f);
        current_item.gameObject.SetActive(true);
    }

    private IEnumerator IntroCoroutine()
    {
        Vector2 intro_start = new Vector3(tile_position.x, tile_position.y + 4) * ground_sprite.bounds.size.x;
        float intro_distance = Vector3.Distance(intro_start, tile_position * MapManager.GroundTileSize);

        SpriteRenderer _spre = GetComponent<SpriteRenderer>();

        while (transform.position != tile_position * MapManager.GroundTileSize)
        {
            transform.position = Vector3.MoveTowards(transform.position, tile_position * ground_sprite.bounds.size.x, fall_speed * Time.deltaTime);
            _spre.color = new Color(1, 1, 1, 1 - Vector2.Distance(transform.position, tile_position * MapManager.GroundTileSize) / intro_distance);
            yield return new WaitForEndOfFrame();
        }

        Camera.main.GetComponent<CameraShake>().ShakeCamera(2.0f, 0.1f);
        SetPlayerState(Enums.PlayerStates.IDLE);
    }

    private IEnumerator OutroCoroutine()
    {   
        while(player_state != Enums.PlayerStates.IDLE)
        {
            yield return new WaitForEndOfFrame();
        }
        player_state = Enums.PlayerStates.OUTRO;

        spre.sortingOrder = -(int)tile_position.y;
        
        while(spre.color.a > 0)
        {   
            spre.color = Vector4.MoveTowards(spre.color, new Vector4(1, 1, 1, 0), 1.0f * Time.deltaTime);
            transform.position += Vector3.down * fall_speed * Time.deltaTime;
            if (current_item != null) current_item.GetComponent<SpriteRenderer>().color = spre.color;
            yield return new WaitForEndOfFrame();
        }

        if (current_item != null)
        {
            current_item.GetComponent<Item>().SetState(Item.ItemState.DISCARDED_FROM_MAP);
            current_item = null;
            anim_controller.SetBool("holding_item", false);
        }

        g_master.NewMap();

    }
}
