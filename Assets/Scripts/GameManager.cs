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

    //OSC stuff
    [SerializeField]
    private Text countText = null;
    private int count;

    Player player;
    Transform playerTransform;
    GameObject playerObject;
    GameObject firepoint;
    GameObject mouse;

    void Awake()
    {
        _instance = this; //singleton, lets anyobject call it without getting the component stuff

        playerObject = GameObject.Find("Player");
        player = GameObject.Find("Player").GetComponent<Player>();
        playerTransform = player.transform;
        firepoint = GameObject.Find("FirePoint");
        mouse = GameObject.Find("Mouse");
    }

    void Start()
    {
        UnityEngine.Cursor.visible = false;

        //OSC stuff
        OSCHandler.Instance.Init();
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/trigger", "ready");
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 1);

        count = 0;
        setCountText();

        OSCHandler.Instance.SendMessageToClient("pd", "/unity/on", 1);
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

    void setCountText()
    {
        countText.text = "Count: " + count.ToString();

        //************* Send the message to the client...
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/trigger", count);
        //*************
    }

    public Vector2 GetPlayerLocation()
    {
        return (Vector2)playerObject.transform.position;
    }

    public Vector2 FireToCursor()
    {
        return (Vector2)(mouse.transform.position - firepoint.transform.position);
    }
}
