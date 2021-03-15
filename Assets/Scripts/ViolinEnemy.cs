using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolinEnemy : CharaDamage
{
    public Transform player;

    [SerializeField]
    private GameObject musicBullet = null;
    [SerializeField]
    private float deathTransparency = 50, fadeSpeed = 5;
    [SerializeField]
    private BoxCollider2D hurtBox = null;

    private Color transparency; // for death
    private SpriteRenderer spriteRenderer; // also prolly for death
    private float dist = 0f;    // measurement used for keeping @ range.
    private int type; // either 1, 2, 3, or 4. shoots on different beats

    protected void Start()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/violin_spawn", 1);
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if(dist > 5 && dist < 15) {
            Vector3 localPosition = player.transform.position - transform.position;
            localPosition = localPosition.normalized; // The normalized direction in LOCAL space
            transform.Translate(localPosition.x * Time.deltaTime * speed/4, localPosition.y * Time.deltaTime * speed/4,
                                 localPosition.z * Time.deltaTime * speed/4);
        }
    }

    protected override void Die()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/violin_die", 1);
        damageable = false;
        dead = true;
        hurtBox.enabled = false;
        transparency = new Color(1, 1, 1, deathTransparency / 100);
        spriteRenderer.color = transparency;
    }

    protected void Shoot()
    {
        
        GameObject note = Instantiate(musicBullet, transform.position, Quaternion.Euler(0, 0, 0));
        note.transform.parent = this.transform;
    }
}