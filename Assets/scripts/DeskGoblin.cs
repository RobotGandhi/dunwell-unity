using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskGoblin : Enemy
{

    public Sprite damaged_sprite, dead_sprite;
    private SpriteRenderer spre;

    void Start()
    {
        spre = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    public override void TakeDamage()
    {
        HP--;
        // TODO@ Fix this 
        /*
        if (HP == 2)
        {
            spre.sprite = damaged_sprite;
        }
        else
        {
            spre.sprite = dead_sprite;
        }
        */
    }
}
