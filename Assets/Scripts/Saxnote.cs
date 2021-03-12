using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxnote : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float speed = 5, damage = 0, knockback = 0.5f;
    private Rigidbody2D rb2D;
    private NoteType note;  //what to spawn in as
    private Sprite currentSprite;
    [SerializeField]
    public Sprite quarterSprite, halfSprite, wholeSprite;
    

    void Start()
    {
        note = GameManager.Instance.GetNote();
        //set type, image, and variables here
        switch (note)
        {
            case NoteType.quarter:
                Debug.Log("quarter");
                this.GetComponent<SpriteRenderer>().sprite = quarterSprite;
                damage = 1;
                break;

            case NoteType.half:
                Debug.Log("half");
                this.GetComponent<SpriteRenderer>().sprite = halfSprite;
                damage = 2;
                break;

            case NoteType.whole:
                Debug.Log("whole");
                this.GetComponent<SpriteRenderer>().sprite = wholeSprite;
                damage = 4;
                break;
        }

        rb2D = GetComponent<Rigidbody2D>();

        //perfect or not

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
