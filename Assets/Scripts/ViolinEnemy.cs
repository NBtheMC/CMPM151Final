using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolinEnemy : CharaDamage
{
    [SerializeField]
    private GameObject musicBullet = null;

    protected void Start()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/violin_spawn", 1);
    }

    protected override void Update()
    {
        base.Update();
        Shoot();
    }

    protected override void Die()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/violin_die", 1);
    }

    protected void Shoot()
    {
        GameObject note = Instantiate(musicBullet, transform.position, Quaternion.Euler(0, 0, 0));
    }
}
