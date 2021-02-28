using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saxnote : MonoBehaviour
{
    [SerializeField]
    private float speed = 0, damage = 0;
    private Rigidbody2D rb2D;


    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        var dir = GameManager.Instance.FireToCursor();
        dir.Normalize();
        //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb2D.AddForce(dir * speed, ForceMode2D.Impulse);
    }
}
