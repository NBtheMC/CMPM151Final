using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaDamage : Damageable
{
    protected Animator animator;//Future animation stuff

    [SerializeField]
    protected float hitstunLength = 0.1f;
    protected float hitstun = 0f;
    protected bool actionable = true;

    protected Vector2 veloTarget, veloChange;

    public CharaDamage()//For declaring stuff
    {
    }
    public CharaDamage(float newHealth)
    {
        health = newHealth;
    }

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        //countdown
        if (hitstun > 0)
        {
            hitstun -= Time.deltaTime;
        }
        if (hitstun <= 0)
        {
            actionable = true;
        }
    }

    public override void Hurt(float damage, Vector2 hitForce)
    {
        if (damageable)
        {
            hitstun = hitstunLength;
            actionable = false;

            base.Hurt(damage, hitForce);

            rb.AddForce(hitForce, ForceMode2D.Impulse);
            Debug.Log("Hit for " + damage + "\nVector is " + hitForce);
        }
    }
}
