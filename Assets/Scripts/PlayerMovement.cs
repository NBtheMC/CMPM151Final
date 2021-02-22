using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 20f;
    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;

    // Start is called before the first frame update
    void Update()
    {
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }

    // FixedUpdate used for physics and rigidbodies and stuff
    void FixedUpdate()
    {
        //Actual movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
