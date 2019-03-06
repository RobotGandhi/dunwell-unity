using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP;
    public int damage;

    public GameObject remainsPrefab;

    public virtual void TakeDamage() { }
}
