using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType { quarter, half, whole };
public class Player : CharaDamage
{
    [SerializeField]
    private GameObject musicBullet = null;
    [SerializeField]
    private Transform firePoint = null;
    [SerializeField]
    private GameObject sax = null;
    private Vector2 hotSpot = Vector2.zero;
    private Vector2 movement;

    //angle debug stuff
    [SerializeField]
    private float angle = 0;
    [SerializeField]
    private Vector2 thisPos, debugMouse;

    //Note Stuff
    private NoteType currentNote = NoteType.quarter;  //can switch between note types
    private float quarterCountdown;
    private float halfCountdown;
    private float wholeCountdown;
    private float tempo;

    protected void Start()
    {
        //everything else based off of the countdown
        tempo = GameManager.Instance.getTempo();
        quarterCountdown = tempo - .1f;
        halfCountdown = quarterCountdown * 2;
        wholeCountdown = quarterCountdown * 4;
    }

    protected override void Update()
    {
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //animation
        if(movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        movement.Normalize();
        //switch to quarter note
        if (Input.GetKeyDown("1"))
        {
            currentNote = NoteType.quarter;
            GameManager.Instance.ChangeSelect(1);
        }
        //switch to half note
        else if (Input.GetKeyDown("2"))
        {
            currentNote = NoteType.half;
            GameManager.Instance.ChangeSelect(2);
        }
        //switch to whole note
        else if (Input.GetKeyDown("3"))
        {
            currentNote = NoteType.whole;
            GameManager.Instance.ChangeSelect(3);
        }
        //countdowns
        if (quarterCountdown > 0)
        {
            quarterCountdown -= Time.deltaTime;
        }
        if (halfCountdown > 0)
        {
            halfCountdown -= Time.deltaTime;
        }
        if (wholeCountdown > 0)
        {
            wholeCountdown -= Time.deltaTime;
        }
        //left mouse click to shoot. depends on current note
        if (Input.GetMouseButtonDown(0))
        {
            if (currentNote == NoteType.quarter && quarterCountdown <= 0)
            {
                Shoot(NoteType.quarter);
                quarterCountdown = tempo - .1f; //reset countdown after shooting 
            }
            else if (currentNote == NoteType.half && halfCountdown <= 0)
            {
                Shoot(NoteType.half);
                halfCountdown = tempo*2 - .1f; //reset countdown after shooting 
            }
            else if (currentNote == NoteType.whole && wholeCountdown <= 0)
            {
                Shoot(NoteType.whole);
                wholeCountdown = tempo*4 - .1f; //reset countdown after shooting 
            }
        }
        //flip player based on mouse position
        Vector2 mousePos = GameManager.Instance.GetMousePosition();
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            transform.localScale = new Vector3(1f, 1f, 1f);

        //changes sax rotation based on mouse position
        float cursorAngle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * 180 / Mathf.PI;
        thisPos = transform.position; //debug stuff
        debugMouse = mousePos;
        angle = cursorAngle;

        float adjustedAngle;
        //-90 and 90
        if(-90 < cursorAngle && cursorAngle < 90)
        {
            adjustedAngle = (cursorAngle + 90) / 180;
            sax.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-20, 20, adjustedAngle));
        }
        else
        {
            //90->180, -180 -> -90
            //needs to be 1->0.5, 0.5->0
            if (cursorAngle > 0)
            {
                adjustedAngle = (cursorAngle - 90) / 180;//0->0.5
                adjustedAngle = (-adjustedAngle) + 1;//1->0.5
            }
            else
            {
                adjustedAngle = (-cursorAngle - 90) / 180;//0.5->0
            }
            sax.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Lerp(-20, 20, adjustedAngle));
        }

        base.Update();
    }

    protected void FixedUpdate()
    {
        //Actual movement
        //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        veloTarget = new Vector2(movement.x * speed, movement.y * speed);
        veloChange = veloTarget - new Vector2(rb.velocity.x, rb.velocity.y);
        veloChange.x = Mathf.Clamp(veloChange.x, -changeSpeed, changeSpeed);
        rb.AddForce(veloChange, ForceMode2D.Impulse);
    }

    protected void Shoot(NoteType shotNote)
    {
        //shoot either quarter, half, or whole note depending on stuff
        switch (shotNote)
        {
            case NoteType.quarter:
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/quarter_sax", 1);
                break;
            case NoteType.half:
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/half_sax", 1);
                break;
            case NoteType.whole:
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/whole_sax", 1);
                break;
        }
        GameObject note = Instantiate(musicBullet,firePoint.position, Quaternion.Euler(0, 0, 0));
    }

    public NoteType getNote()
    {
        return currentNote;
    }

    public override void Hurt(float damage, Vector2 hitForce)
    {
        if (damageable)
        {
            base.Hurt(damage, hitForce);
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/hurt", 1);
        }
    }

    protected override void Die()
    {
        damageable = false;
        GameManager.Instance.FadeOn();
    }
}
