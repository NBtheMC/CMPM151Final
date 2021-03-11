using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxnote : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float speed = 0, damage = 0, knockback = 0;
    private Rigidbody2D rb2D;
    private NoteType note;  //what to spawn in as

    void Start()
    {
        note = GetComponent<Player>().getNote();
        //set type, image, and variables here
        rb2D = GetComponent<Rigidbody2D>();

        var dir = GameManager.Instance.FireToCursor();
        dir.Normalize();
        rb2D.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable d = collision.transform.root.GetComponent<Damageable>();
        if (d != null)
        {
            Vector2 hitForce = rb2D.velocity;
            hitForce.Normalize();
            hitForce *= knockback;

            if (rb2D.velocity.x < 0)
            {
                hitForce = Vector2.Reflect(hitForce, Vector2.right);
            }
            GameManager.Instance.DamageGeneral(d, damage, hitForce);
        }
        Destroy(this.gameObject);//Add in an animation and add it to a pool for better performance
    }
}
