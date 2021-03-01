using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaDamage
{

    [SerializeField]
    private GameObject musicBullet = null;
    [SerializeField]
    private Transform firePoint = null;
    private float countdown = .10f;
    private Vector2 hotSpot = Vector2.zero;

    Vector2 movement;

    protected override void Update()
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
        base.Update();
    }

    protected void FixedUpdate()
    {
        //Actual movement
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    protected void Shoot()
    {
        GameObject note = Instantiate(musicBullet,firePoint.position, Quaternion.Euler(0, 0, 0));
    }
}
