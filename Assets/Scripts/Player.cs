using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;
    private Rigidbody2D rb;

    //shooting stuff (yes, in the movement script, sue me)
    [SerializeField]
    private GameObject musicBullet;
    [SerializeField]
    private Camera cam;
    private float bulletForce = 20f;
    private float countdown = .10f;

    Vector2 movement;
    Vector2 mousePos;  //cursor
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Update()
    {
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        //countdown
        if(countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        //left mouse click to shoot
        if (Input.GetMouseButtonDown(0) && countdown <= 0)
        {
            Shoot();
            countdown = .10f; //reset countdown after shooting 
        }
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    // FixedUpdate used for physics and rigidbodies and stuff
    void FixedUpdate()
    {
        //Actual movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //actually shooting
        Vector2 shootDir = mousePos - rb.position; //subtracting vectors gets one that points from one to the other
    }

    void Shoot()
    {
        GameObject note = Instantiate(musicBullet,GetComponent<Transform>().position, GetComponent<Transform>().rotation);
        Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
        rb.AddForce(GetComponent<Transform>().position* bulletForce,ForceMode2D.Impulse);

    }
}
