using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP;
    public int damage;
    public GameObject remainsPrefab;

    [System.NonSerialized]
    public Vector2 tile_position;
    [System.NonSerialized]
    public MapManager.TileValues tile_value;
    [System.NonSerialized]
    public MapManager.TileValues tile_value_under;

    protected GameMaster game_master;
    protected SpriteRenderer spre;
    protected Player player;

    public void BaseStart()
    {
        game_master = FindObjectOfType<GameMaster>();
        spre = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
    }

    // Called right after taking some damage
    public virtual void TakeDamage() { }
    // Called right after the player has performed his action
    public virtual void PlayerEvent() { }
}
