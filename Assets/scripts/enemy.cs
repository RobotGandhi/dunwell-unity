using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP;
    public int damage;
    public Vector2 tile_position;
    public MapManager.TileValues tile_value;

    public GameObject remainsPrefab;

    protected GameMaster game_master;
    protected SpriteRenderer spre;

    public void BaseStart()
    {
        game_master = FindObjectOfType<GameMaster>();
        spre = FindObjectOfType<SpriteRenderer>();
    }

    // Called right after taking some damage
    public virtual void TakeDamage() { }
    // Called right after the player has performed his movement
    public virtual void Step() { }
}
