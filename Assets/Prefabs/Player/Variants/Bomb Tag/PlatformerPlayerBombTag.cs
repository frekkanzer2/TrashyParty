using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerBombTag : PlatformerPlayer
{

    public GameObject AttachedBomb = null;

    protected override void VariantStart()
    {

    }

    protected override void VariantUpdate()
    {
        if (this.HasBomb()) AttachedBomb.transform.position = this.transform.position.Variation(0, 2, 0);
    }

    protected override void VariantFixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.IsGameEnded() && this.HasBomb() && AttachedBomb.GetComponent<BombTagBehaviour>().CanBeAttached())
            collision.gameObject.GetComponent<PlatformerPlayerBombTag>().PassBomb(this, this.AttachedBomb);
    }

    public bool HasBomb() => AttachedBomb != null;

    public void PassBomb(PlatformerPlayerBombTag attacker, GameObject bomb)
    {
        this.AttachedBomb = bomb;
        if (attacker != null) attacker.AttachedBomb = null;
        bomb.GetComponent<BombTagBehaviour>().OnAssigned();
    }

}
