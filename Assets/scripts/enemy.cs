using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [System.NonSerialized]
    public int HP;
    [System.NonSerialized]
    public int damage;

    public virtual void TakeDamage() { }
}
