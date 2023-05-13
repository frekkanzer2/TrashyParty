using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerTrapRun : PlatformerPlayer
{

    private bool isTempDeath = false;
    private Animator bodyAnimator = null;
    private Vector3 respawnPosition = Vector3.zero;

    protected override void VariantStart()
    {
        base.VariantStart();
        bodyAnimator = body.GetChild(0).gameObject.GetComponent<Animator>();
        respawnPosition = this.transform.position;
    }

    protected override void VariantUpdate()
    {
    }

    protected override void VariantFixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead || isTempDeath) return;
        if (collision.gameObject.layer == Constants.LAYER_DEADZONE && !GameManager.Instance.IsGameEnded())
        {
            OnTempDeath();
            DestroyableObstacle obstacle = collision.gameObject.GetComponent<DestroyableObstacle>();
            if (obstacle is not null) obstacle.RegisterCollision();
        } else if (collision.gameObject.name == "downdoor")
        {
            IPlayer winner = this;
            List<TeamDto> loserTeams = GameManager.Instance.Teams.FindAll(t => t.GetAlivePlayers().Count > 0 && !t.players[0].Equals(winner));
            foreach (TeamDto team in loserTeams)
                ((PlatformerPlayerTrapRun)team.players[0]).OnRealDeath();
        }
    }

    public override void OnDeath()
    {
        // overrided to nothing happens
    }

    public void OnTempDeath()
    {
        if (isDead || isTempDeath) return;
        SetConfusion(false);
        SoundsManager.Instance.PlayPlayerSound(ISoundsManager.PlayerSoundType.Dead);
        isTempDeath = true;
        this.canWalk = false;
        this.canJump = false;
        bodyAnimator.enabled = false;
        changeSprite(deathSprite);
        head.gameObject.SetActive(false);
        StartCoroutine(WaitRespawn());
    }
    IEnumerator WaitRespawn()
    {
        yield return new WaitForSeconds(2);
        if (!isDead)
        {
            this.transform.position = respawnPosition;
            isTempDeath = false;
            this.canWalk = true;
            this.canJump = true;
            changeSprite(birdSprite);
            bodyAnimator.enabled = true;
            head.gameObject.SetActive(true);
        }
    }
    public void OnRealDeath() => base.OnDeath();

}
