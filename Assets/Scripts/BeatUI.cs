using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatUI : MonoBehaviour
{
    public GameObject ShootingTiming;
    private GameObject finishline = GameObject.Find("lineStop");
    private Vector2 direction = new Vector2(3.61f, 0);
    private float tempo;
    private int beat;
    // Start is called before the first frame update
    void Start()
    {
        ShootingTiming = GameManager.Instance.GetShootingTiming();
        tempo = GameManager.Instance.getTempo();
        beat = GameManager.Instance.GetShootingTiming().GetComponent<ShootingTiming>().getBeat();

    }

    // Update is called once per frame
    void Update()
    {
        // get the tempo timing per millisecond
        // get transform of LineStart
        transform.Translate(direction / tempo * Time.deltaTime);
    }
    void FixedUpdate() {
        // get beat
        // if = 0, reset transform to Vector3(-422.34, 18.23, 0)
        if(Vector3.Distance(finishline.GetComponent<Transform>.position, transform.position) == 0) {
            transform = new Vector3(-422.34f, 18.23f, 0f);
        }

    }
}
