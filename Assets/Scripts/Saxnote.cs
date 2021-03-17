using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxnote : MonoBehaviour
{
    [SerializeField]
    private float speed = 5, qdamage = 0, hdamage = 0, wdamage, knockback = 0.5f;
    private float damage = 0;
    private Rigidbody2D rb2D;
    private NoteType note;  //what to spawn in as
    private Sprite currentSprite;
    private bool pierce = false;
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
                damage = qdamage;
                break;

            case NoteType.half:
                Debug.Log("half");
                this.GetComponent<SpriteRenderer>().sprite = halfSprite;
                damage = hdamage;
                knockback *= 1.5f;
                break;

            case NoteType.whole:
                Debug.Log("whole");
                this.GetComponent<SpriteRenderer>().sprite = wholeSprite;
                damage = wdamage;
                pierce = true;
                knockback *= 2;
                break;
        }

        rb2D = GetComponent<Rigidbody2D>();

        //modify the note if its shot on time
        if (GameManager.Instance.GetShootingTiming().GetComponent<ShootingTiming>().isPerfect())
        {
            damage *= 1.5f;
            this.transform.localScale *= new Vector2(1.5f,1.5f);
            //make sound
        }
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

            GameManager.Instance.DamageGeneral(d, damage, hitForce);
            if(!pierce)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);//Add in an animation and add it to a pool for better performance
        }
    }
}
