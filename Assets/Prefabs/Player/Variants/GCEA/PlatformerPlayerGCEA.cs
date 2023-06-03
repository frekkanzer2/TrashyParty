using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerGCEA : PlatformerPlayer
{

    public bool IsCatcher { get; set; }
    public GameObject CatcherSign;

    protected override void VariantStart()
    {
        base.VariantStart();
        IsCatcher = false;
        CatcherSign.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.IsGameEnded())
            if (this.IsCatcher && !collision.gameObject.GetComponent<PlatformerPlayerGCEA>().IsCatcher) collision.gameObject.GetComponent<IPlayer>().OnDeath();
    }

    public void TurnAsCatcher()
    {
        this.ChangePlayerStats(((int)AppSettings.Get("N_PLAYERS") <= 6) ? Constants.PLAYER_MOVEMENT_SPEED - 1 : Constants.PLAYER_MOVEMENT_SPEED - 2, Constants.PLAYER_JUMPING_POWER);
        this.IsCatcher = true;
        CatcherSign.SetActive(true);
    }

    protected override void VariantUpdate()
    {
        base.VariantUpdate();
        if (this.IsCatcher)
        {
            if (this.body.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX) this.CatcherSign.transform.localPosition = new(-0.576f, 1.039f, 0);
            else this.CatcherSign.transform.localPosition = new(0.576f, 1.039f, 0);
        }
    }

}
