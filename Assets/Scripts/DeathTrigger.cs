using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : CollidableObject
{
    public override void CollisionEnter(Collision2D collision)
    {
    }

    public override void TriggerEnter(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>())
        {
            FindObjectOfType<DeathManager>().KillPlayer();
        }
    }
}
