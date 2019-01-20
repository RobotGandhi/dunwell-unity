using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeleton : enemy
{

    public Sprite damaged_sprite, dead_sprite;
    private SpriteRenderer spre;

    void Start()
    {
        spre = GetComponent<SpriteRenderer>();
        HP = 2;
        damage = 1;
    }

    void Update()
    {
        
    }

    public override void TakeDamage()
    {
        HP--;
        if(HP == 2)
        {
            spre.sprite = damaged_sprite;
        }
        else
        {
            spre.sprite = dead_sprite;
        }
    }
}
