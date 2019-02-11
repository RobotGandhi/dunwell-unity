using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        WEAPON,
        SHIELD,
        HEALTH
    }

    public enum ItemState
    {
        ON_MAP,
        PICKED_UP,
        DISCARDED_FROM_MAP
    }

    [System.NonSerialized]
    public ItemType item_type;
    public ItemState item_state = ItemState.ON_MAP;

    public void SetState(ItemState state)
    {
        item_state = state;

        if(item_state == ItemState.DISCARDED_FROM_MAP)
            GetComponent<SpriteRenderer>().enabled = false;
    }
}
