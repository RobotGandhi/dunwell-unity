using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
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
