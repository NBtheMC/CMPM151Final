using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
    }
}