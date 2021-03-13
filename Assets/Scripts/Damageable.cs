using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    protected float health, speed;
    protected bool damageable = true, dead = false;

    protected Rigidbody2D rb;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Hurt(float damage, Vector2 hitForce)
    {
        if (damageable)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    protected virtual void Die()
    {
        damageable = false;
        Debug.Log(this.gameObject.name + " is dead");
    }
}
