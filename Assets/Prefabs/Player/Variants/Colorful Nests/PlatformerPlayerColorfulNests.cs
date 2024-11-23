using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerColorfulNests : PlatformerPlayer
{

    protected override void OnTriggerEnterOverridable(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickable") && !this.IsDead())
        {
            NestBehaviour nb = collision.gameObject.GetComponent<NestBehaviour>();
            nb.CollisionWithPlayer(this);
        }
        base.OnTriggerEnterOverridable(collision);
    }

}
