using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolinEnemy : CharaDamage
{
    [SerializeField]
    private GameObject musicBullet = null;
    [SerializeField]
    private float deathTransparency = 50, fadeSpeed = 5;
    private Color transparency;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D hurtBox = null;

    protected void Start()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/violin_spawn", 1);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
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
            Shoot();
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
    }
}
