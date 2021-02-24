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
    [SerializeField]
    private Transform firePoint;
    private float bulletForce = 1f;
    private float countdown = .10f;
    //cursor stuff
    [SerializeField]
    private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    Vector2 movement;
    Vector2 mousePos;  //cursor
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
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
        firePoint.transform.LookAt(mousePos);
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
        GameObject note = Instantiate(musicBullet,firePoint.position, firePoint.rotation);
        Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.position* bulletForce,ForceMode2D.Impulse);

    }
}
