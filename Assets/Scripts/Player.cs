using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharaDamage
{

    [SerializeField]
    private GameObject musicBullet = null;
    [SerializeField]
    private Transform firePoint = null;
    private Vector2 hotSpot = Vector2.zero;
    Vector2 movement;

    //Note Stuff
    public enum NoteType { quarter, half, whole };
    public NoteType currentNote = NoteType.quarter;  //can switch between note types
    private float tempo; //the master tempo, a bit more than the cooldown
    private bool perfect; //if on tempo. need to get bangs from Pure data
    private float quarterCountdown;
    private float halfCountdown;
    private float wholeCountdown;

    protected void Start()
    {
        //everything else based off of the countdown
        tempo = 1.5f;
        quarterCountdown = tempo - .1f;
        halfCountdown = quarterCountdown * 2;
        wholeCountdown = quarterCountdown * 4;
    }

    protected override void Update()
    {
        //recieve pure data stuff and update accordingly
        //uhhhhh I (Naman) do not know how to recieve from pd yet. I will do this tomorrow 3/9

        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        //switch to quarter note
        if (Input.GetKeyDown("1"))
        {
            currentNote = NoteType.quarter;
        }
        //switch to half note
        if (Input.GetKeyDown("2"))
        {
            currentNote = NoteType.half;
        }
        //switch to whole note
        if (Input.GetKeyDown("3"))
        {
            currentNote = NoteType.whole;
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
        base.Update();
    }

    protected void FixedUpdate()
    {
        //Actual movement
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    protected void Shoot(NoteType shotNote)
    {
        //shoot either quarter, half, or whole note depending on stuff
        GameObject note = Instantiate(musicBullet,firePoint.position, Quaternion.Euler(0, 0, 0));
    }
}
