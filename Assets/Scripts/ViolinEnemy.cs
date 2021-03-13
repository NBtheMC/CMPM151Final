using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolinEnemy : CharaDamage
{
    protected void Start()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/violin_spawn", 1);
    }

    protected override void Die()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/violin_die", 1);
    }

    protected void Shoot()
    {

    }
}
