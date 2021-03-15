using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles shooting timing of enemies and the player
public class ShootingTiming : MonoBehaviour
{
    private float tempo;
    private int beat = 4; //either 1,2,3, or 4. used to shoot appropriate event
    public UnityEngine.Events.UnityEvent EnemiesToShoot; //triggers shooting on whatever enemies
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
    }

    // Update is called once per frame
    void Update()
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
        if (tempo > 0)
        {
            tempo -= Time.deltaTime;
        }
        else
        {
            //update tempo on beat
            tempo = GameManager.Instance.getTempo();
            //set beat
            beat = ((beat + 1)%4)+1;
            //shoot based on beat

        }
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
