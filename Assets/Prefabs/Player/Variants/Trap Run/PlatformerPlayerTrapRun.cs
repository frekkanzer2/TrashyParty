using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerTrapRun : PlatformerPlayer
{

    private bool isTempDeath = false;
    private Animator bodyAnimator = null;
    public bool DEBUG;

    protected override void VariantStart()
    {
        base.VariantStart();
        RespawnPosition = this.transform.position;
    }

    protected override void VariantUpdate()
    {
        if (DEBUG)
            Debug.Log($"STATUS OF PLAYER: IsDead {this.IsDead()} | IsTempDeath {this.isTempDeath} | CanJump {this.canJump} | CanWalk {this.canWalk}");
        if (bodyAnimator is null)
        {
            try
            {
                bodyAnimator = body.GetChild(0).gameObject.GetComponent<Animator>();
            } catch (UnityException) { }
        }
    }

    protected override void VariantFixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead || isTempDeath) return;
        int layer = collision.gameObject.layer;
        Transform collisionTransform = collision.gameObject.transform;
        if (layer == Constants.LAYER_DEADZONE && !GameManager.Instance.IsGameEnded())
        {
            OnTempDeath();
            DestroyableObstacle obstacle = collision.gameObject.GetComponent<DestroyableObstacle>();
            if (obstacle is not null) obstacle.RegisterCollision();
        }
        PlatformerPlayer collidedPlayer;
        try
        {
            collidedPlayer = collisionTransform.parent.gameObject.GetComponent<PlatformerPlayer>();
        }
        catch (System.NullReferenceException)
        {
            return;
        }
        if (layer == Constants.LAYER_PLAYERHEAD && this.transform.position.y >= collisionTransform.position.y && !GameManager.Instance.IsGameEnded())
        {
            if (collidedPlayer != null)
            {
                if (
                    this.canConfuseOtherBirds && 
                    !this.IsConfused && 
                    !collidedPlayer.IsConfused && 
                    !collidedPlayer.IsDead() && 
                    !((PlatformerPlayerTrapRun)collidedPlayer).isTempDeath
                    && GameManager.Instance.IsGameStarted()
                ) collidedPlayer.SetConfusion(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "downdoor")
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
            this.transform.position = RespawnPosition;
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
