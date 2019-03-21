using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    public Sprite down_sprite;
    [System.NonSerialized]
    public bool toggled = false;
    public Vector2 gate_position;

    private GameMaster game_master;

    private void Awake()
    {
        game_master = FindObjectOfType<GameMaster>();
    }

    public void Enable()
    {
        if (!toggled)
        {
            toggled = true;

            // Change sprite!
            GetComponent<SpriteRenderer>().sprite = down_sprite;
            // SFX
            FindObjectOfType<SoundEffects>().PlaySFX("pp1");
            // Open the gate!
            game_master.current_map.gate_map[gate_position].Open();
        }
    }

}
