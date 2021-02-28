using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject musicBullet = null;
    [SerializeField]
    private Transform firePoint = null;
    private float countdown = .10f;
    private Vector2 hotSpot = Vector2.zero;

    Vector2 movement;
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
    }

    // FixedUpdate used for physics and rigidbodies and stuff
    void FixedUpdate()
    {
        //Actual movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject note = Instantiate(musicBullet,firePoint.position, Quaternion.Euler(0, 0, 0));
    }
}
