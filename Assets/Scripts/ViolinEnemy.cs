using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolinEnemy : CharaDamage
{
    private Transform player;

    [SerializeField]
    private GameObject shootingTiming;

    [SerializeField]
    private GameObject musicBullet = null;
    [SerializeField]
    private float deathTransparency = 50, fadeSpeed = 5;
    [SerializeField]
    private BoxCollider2D hurtBox = null;
    [SerializeField]
    private BoxCollider2D pushBox = null;

    private Color transparency; // for death
    private SpriteRenderer spriteRenderer; // also prolly for death
    private float dist = 0f;    // measurement used for keeping @ range.
    public int type; // either 1, 2, 3, or 4. shoots on different beats

    protected void Start()
    {
        player = GameManager.Instance.GetPlayerTransform();
        shootingTiming = GameManager.Instance.GetShootingTiming();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootingTiming.GetComponent<ShootingTiming>().OnBeat += ShootingTiming_OnBeat;
    }

    public void setType(int newType)
    {
        type = newType;
    }

    private void ShootingTiming_OnBeat(object sender, EventArgs e)
    {
        if (!dead)
        {
            //shoot based on beat
            switch (shootingTiming.GetComponent<ShootingTiming>().getBeat())
            {
                case 1:
                    if (type == 1 || type == 2)
                    {
                        Shoot();
                    }
                    break;
                case 2:
                    if (type == 4)
                    {
                        Shoot();
                    }
                    break;
                case 3:
                    if (type == 3 || type == 2)
                    {
                        Shoot();
                    }
                    break;
                case 4:
                    if (type == 4)
                    {
                        Shoot();
                    }
                    break;
            }
        }
    }

    protected override void Update()
    {
        dist = Vector3.Distance(player.position, transform.position);
        if (dead)
        {
            if (spriteRenderer.color.a <= 0)
                Destroy(gameObject);
            else
                spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - fadeSpeed/100 * Time.deltaTime);
        }
        else
        {
            base.Update();
            //Shoot();
        }
    }

    public void FixedUpdate() {
        if(dist > 5 && dist < 15 && !dead) {
            Vector3 localPosition = player.transform.position - transform.position;
            localPosition = localPosition.normalized; // The normalized direction in LOCAL space
            transform.Translate(localPosition.x * Time.deltaTime * speed/4, localPosition.y * Time.deltaTime * speed/4,
                                 localPosition.z * Time.deltaTime * speed/4);
        }
    }

    protected override void Die()
    {      
        damageable = false;
        dead = true;
        hurtBox.enabled = false;
        pushBox.enabled = false;
        transparency = new Color(1, 1, 1, deathTransparency / 100);
        spriteRenderer.color = transparency;
    }

    protected void Shoot()
    {
        //check type again to send appropriate bang to pd
        switch (type)
        {
            case 1:
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/violin1", 1);
                break;
            case 2:
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/violin2", 1);
                break;
            case 3:
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/violin3", 1);
                break;
            case 4:
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/violin4", 1);
                break;
        }
        GameObject note = Instantiate(musicBullet, transform.position, Quaternion.Euler(0, 0, 0));
        note.transform.parent = this.transform;

        //animation stuff
        if (animator.GetBool("atk2"))
        {
            animator.SetBool("atk2", false);
        }
        else
        {
            animator.SetBool("atk2", true);
        }
        animator.SetTrigger("atk");
    }
}