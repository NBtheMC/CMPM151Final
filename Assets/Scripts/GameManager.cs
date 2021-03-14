using System.Collections;
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
    private float tempo; //each beat, in milliseconds
    private float aheadCountdown; //when gets to 0 turns on perfect
    private float behindCountdown; //when gets to 0 turns off perfect
    private bool perfect = false;


    Player player;
    GameObject playerObject;
    GameObject firepoint;
    GameObject mouse;
    GameObject uiSelect;

    //UI select stuff
    [SerializeField]
    private GameObject Select1 = null;
    [SerializeField]
    private GameObject Select2 = null;
    [SerializeField]
    private GameObject Select3 = null;

    void Awake()
    {
        _instance = this; //singleton, lets anyobject call it without getting the component stuff

        playerObject = GameObject.Find("Player");
        player = GameObject.Find("Player").GetComponent<Player>();
        firepoint = GameObject.Find("FirePoint");
        mouse = GameObject.Find("Mouse");
        uiSelect = GameObject.Find("UI Select");
    }

    void Start()
    {
        UnityEngine.Cursor.visible = false;

        //OSC stuff
        OSCHandler.Instance.Init();
        //time stuff up right
        aheadCountdown = .95f * tempo;
        behindCountdown = 1.05f * tempo;
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start_stop", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/tempo", tempo);
    }

    //countdowns
    private void Update()
    {
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

    public Vector2 GetPlayerLocation()
    {
        return (Vector2)playerObject.transform.position;
    }

    public Vector2 FireToCursor()
    {
        return (Vector2)(mouse.transform.position - firepoint.transform.position);
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
    public bool isPerfect()
    {
        return perfect;
    }
}