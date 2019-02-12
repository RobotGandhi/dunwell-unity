﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums 
{

    public enum PlayerStates
    {
        IDLE,
        MOVING,
        INTRO,
        DEAD,
        OUTRO
    }

    public enum PlayerMoveDirection
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }

}
