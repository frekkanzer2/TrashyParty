using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerLaserDodge : TopDownPlayer
{
    protected override void VariantStart()
    {
        base.VariantStart();
        RespawnPosition = this.transform.position;
    }

    protected override void VariantUpdate()
    {
    }

    protected override void VariantFixedUpdate()
    {
    }
    protected override void OnTriggerEnterOverridable(Collider2D collision)
    {

        if (this.isDead) return;

        int layer = collision.gameObject.layer;
        string tag = collision.gameObject.tag;

        if (tag == "Pickable" && !GameManager.Instance.IsGameEnded())
        {
            Debug.Log($"Team {this.Team} picked laser");
            ILaser laser = collision.gameObject.GetComponent<ILaser>();
            laser.OnPlayerCollision(this);
        } else if (layer == Constants.LAYER_SPECIAL_DEADZONE)
        {
            ILaser laser = collision.gameObject.transform.parent.parent.GetComponent<ILaser>(); // getting laser component from ray
            if (this.Team != laser.Team && laser.IsAlive) this.OnDeath();
        }

    }
}
