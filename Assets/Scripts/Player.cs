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
    private bool fastTempo = false;

    //angle debug stuff
    [SerializeField]
    private float angle = 0, saxExpand = 1.5f, saxExpandSpeed = 0.1f;//multiplier
    [SerializeField]
    private Vector2 thisPos, debugMouse;

    //Note Stuff
    private float tempo, baseHealth;
    private NoteType currentNote = NoteType.quarter;  //can switch between note types
    private float currentCountdown; //keeps track of when player can shoot notes again based off of tempo
    

    protected void Start()
    {
        //shooting time based off of the tempo
        tempo = GameManager.Instance.getTempo();
        currentCountdown = 0;

        baseHealth = health;
    }

    protected override void Update()
    {
        tempo = GameManager.Instance.getTempo();

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
        //countdown
        if (currentCountdown > 0)
        {
            currentCountdown -= Time.deltaTime;
        }
        //left mouse click to shoot. depends on current note
        if (Input.GetMouseButtonDown(0) && currentCountdown <= 0)
        {
            if (currentNote == NoteType.quarter)
            {
                Shoot(NoteType.quarter);
                currentCountdown = tempo - .1f; //reset countdown after shooting 
            }
            else if (currentNote == NoteType.half)
            {
                Shoot(NoteType.half);
                currentCountdown = tempo*2 - .1f; //reset countdown after shooting 
            }
            else if (currentNote == NoteType.whole)
            {
                Shoot(NoteType.whole);
                currentCountdown = tempo*4 - .1f; //reset countdown after shooting 
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

        //expand sax
        if(currentCountdown > 0)
        {
            if(sax.transform.localScale.x < saxExpand)
            {
                sax.transform.localScale = new Vector3(sax.transform.localScale.x+saxExpandSpeed, 1, 1);
            }
        }
        else//deflate sax
        {
            if (sax.transform.localScale.x > 1)
            {
                sax.transform.localScale = new Vector3(sax.transform.localScale.x - saxExpandSpeed, 1, 1);
            }
            else
            {
                sax.transform.localScale = new Vector3(1, 1, 1);
            }
        }
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

        if(health <= (baseHealth/2) && !fastTempo)
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot/low_health", 1);
            fastTempo = true;
        }
    }

    protected override void Die()
    {
        damageable = false;
        GameManager.Instance.FadeOn();
    }
}
