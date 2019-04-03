using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemy : Enemy
{
    public enum GuardDirection
    {
        UP = MapManager.TileValues.GUARD_UP,
        LEFT = MapManager.TileValues.GUARD_LEFT
    }

    [System.NonSerialized]
    public GuardDirection guardDirection;
}
