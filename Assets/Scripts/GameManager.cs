﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance //Singleton Stuff
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Game Manager is Null");
            }
            return _instance;
        }
    }

    //OSC stuff I dont THINK we need this but its connected to some server stuff so dont risk it
    [SerializeField]
    private Text countText = null;
    private int count;

    //tempo stuff
    [SerializeField]
    private float tempo = 0.4f, fadeTime = 5; //each beat, in seconds
    private float fadeLerp = 0;
    private bool fadeOff = true; 

    Player player;
    GameObject playerObject;
    GameObject firepoint;
    GameObject mouse;
    GameObject uiSelect;
    GameObject ShootingTiming;

    //UI select stuff
    [SerializeField]
    private GameObject Select1 = null, Select2 = null, Select3 = null;

    [SerializeField]
    private Image fade = null;

    void Awake()
    {
        _instance = this; //singleton, lets anyobject call it without getting the component stuff

        playerObject = GameObject.Find("Player");
        player = GameObject.Find("Player").GetComponent<Player>();
        firepoint = GameObject.Find("FirePoint");
        mouse = GameObject.Find("Mouse");
        uiSelect = GameObject.Find("UI Select");
        ShootingTiming = GameObject.Find("ShootingTiming");
        //OSC stuff
        OSCHandler.Instance.Init();
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", tempo * 1000);
        fade.enabled = true;
        fadeOff = true;
    }

    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    void Update()
    {
        if(fadeOff && fade.color.a > 0)
        {
            fade.color = Color.Lerp(Color.black, Color.clear, fadeLerp);
            if (fadeLerp < 1)
                fadeLerp += Time.deltaTime / fadeTime;
        }
        else if (!fadeOff && fade.color.a < 1)
        {
            fade.color = Color.Lerp(Color.clear, Color.black, fadeLerp);
            if (fadeLerp < 1)
                fadeLerp += Time.deltaTime / fadeTime;
        }
    }

    void FixedUpdate()
    {
        //OSC stuff
        OSCHandler.Instance.UpdateLogs();
        Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
        servers = OSCHandler.Instance.Servers;

        foreach (KeyValuePair<string, ServerLog> item in servers)
        {
            // If we have received at least one packet,
            // show the last received from the log in the Debug console
            if (item.Value.log.Count > 0)
            {
                int lastPacketIndex = item.Value.packets.Count - 1;

                //get address and data packet
                countText.text = item.Value.packets[lastPacketIndex].Address.ToString();
                countText.text += item.Value.packets[lastPacketIndex].Data[0].ToString();

            }
        }
    }

    public Transform GetPlayerTransform()
    {
        return player.transform;
    }

    public Vector2 GetPlayerLocation()
    {
        return (Vector2)playerObject.transform.position;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    public Vector2 FireToCursor()
    {
        return (Vector2)(mouse.transform.position - firepoint.transform.position);
    }

    public GameObject GetShootingTiming()
    {
        return ShootingTiming;
    }
    public void DamageGeneral(Damageable d, float damage, Vector2 hitForce)//attack tells GM to damage target.
    {
        d.Hurt(damage, hitForce);
    }

    public NoteType GetNote()
    {
        return player.getNote();
    }

    public void ChangeSelect(float note)//1= quarter, 2= half, 3=whole
    {
        switch (note)
        {
            case 1:
                uiSelect.transform.position = Select1.transform.position;
                break;
            case 2:
                uiSelect.transform.position = Select2.transform.position;
                break;
            case 3:
                uiSelect.transform.position = Select3.transform.position;
                break;
            default:
                Debug.Log("Note not in UI");
                break;
        }
    }

    public float getTempo()
    {
        return tempo;
    }
    public void setTempo(float newTempo)
    {
        tempo = newTempo;
        return;
    }

    public void FadeOff()
    {
        fadeTime /= 2;
        fadeOff = true;
        fadeLerp = 0;
    }

    public void FadeOn()
    {
        fadeTime *= 2;
        fadeOff = false;
        fadeLerp = 0;
    }
}