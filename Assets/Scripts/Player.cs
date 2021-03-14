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
    private Vector2 hotSpot = Vector2.zero;
    Vector2 movement;

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
        //update perfect signal based on pure data


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
        switch (shotNote)
        {
            case NoteType.quarter:
                break;
            case NoteType.half:
                break;
            case NoteType.whole:
                break;
        }
        GameObject note = Instantiate(musicBullet,firePoint.position, Quaternion.Euler(0, 0, 0));
    }
    public NoteType getNote()
    {
        return currentNote;
    }
}
