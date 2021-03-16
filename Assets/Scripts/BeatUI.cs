using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatUI : MonoBehaviour
{
    public GameObject ShootingTiming;

    // Start is called before the first frame update
    void Start()
    {
        ShootingTiming = GameManager.Instance.GetShootingTiming();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
