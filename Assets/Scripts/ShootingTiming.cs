﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles shooting timing of enemies and the player
//iCarly pog??
public class ShootingTiming : MonoBehaviour
{
    private float tempo;
    private int beat = 4; //either 1,2,3, or 4. used to shoot appropriate event
    public event EventHandler OnBeat; //when beat happens
    public event EventHandler OnChangeTempo; //when tempo changes
    private float aheadCountdown; //when gets to 0 turns on perfect
    private float behindCountdown; //when gets to 0 turns off perfect
    private bool perfect = false; //rewards player for shooting on beat

    // Start is called before the first frame update
    void Start()
    {
        //time stuff up right
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start_stop", 1);
        tempo = GameManager.Instance.getTempo();
        aheadCountdown = .95f * tempo;
        behindCountdown = 1.05f * tempo;
        OnBeat += Testing_OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
        //perfect stuff
        if (aheadCountdown > 0)
        {
            aheadCountdown -= Time.deltaTime;
        }
        else
        {
            perfect = true;
            aheadCountdown = tempo;
        }
        if (behindCountdown > 0)
        {
            behindCountdown -= Time.deltaTime;
        }
        else
        {
            perfect = false;
            behindCountdown = tempo;
        }

        //on beat stuff
        if (tempo > 0)
        {
            tempo -= Time.deltaTime;
        }
        else
        {
            //change tempo
            if(tempo != GameManager.Instance.getTempo())
            {
                tempo = GameManager.Instance.getTempo();
                OnChangeTempo?.Invoke(this,EventArgs.Empty);
            }
            
            //set beat based on tempo
            beat = (beat%4)+1;
            //shoot based on beat
            OnBeat?.Invoke(this,EventArgs.Empty); //calls onbeat event if its not null
        }
    }

    void OnApplicationQuit()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start_stop", 1);
    }

    public void Testing_OnBeat(object sender, EventArgs e)
    {
        Debug.Log("Beat: " + getBeat());
    }
   
    //make sure everything stays on beat
    public void setTempo(float newTempo)
    {
        tempo = newTempo;
    }
    public float getTempo()
    {
        return tempo;
    }
    public int getBeat()
    {
        return beat;
    }
    //if a click is close to on beat. called by player
    public bool isPerfect()
    {
        return perfect;
    }
}