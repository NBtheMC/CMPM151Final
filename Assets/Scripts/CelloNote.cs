using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloNote : MonoBehaviour
{
    //Same thing as Saxnote except fires to player. If I was less lazy I would instead make a base class.
    [SerializeField]
    private float speed = 5, damage = 0, knockback = 2f;
    private Rigidbody2D rb2D;
    private NoteType note;  //what to spawn in as
    private Sprite currentSprite;
    [SerializeField]
    public Sprite wholeSprite;


    void Start()
    {
        //spawn in as whole every time
        Debug.Log("whole");
        this.GetComponent<SpriteRenderer>().sprite = wholeSprite;
        damage = 3;

        rb2D = GetComponent<Rigidbody2D>();

        var dir = GameManager.Instance.GetPlayerLocation() - (Vector2)this.transform.position;
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
        }
        Destroy(this.gameObject);//Add in an animation and add it to a pool for better performance
    }
}
