using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerStaticStun : PlatformerPlayer
{

    private float ForcePower = 25;
    private float VelocityResetSpeed = 0.5f;
    public GameObject PrefabStaticStunSphere;

    void FixedUpdate()
    {
        if (IsConfused)
        {
            if (rigidbody.velocity.x > VelocityResetSpeed)
                rigidbody.velocity = Extensions.Vector3.FromVector2(rigidbody.velocity).Variation(-VelocityResetSpeed, 0, 0);
            else if (rigidbody.velocity.x < VelocityResetSpeed)
                rigidbody.velocity = Extensions.Vector3.FromVector2(rigidbody.velocity).Variation(VelocityResetSpeed, 0, 0);
        }
        if (!IsInitialized || isDead || !canPlay || _isConfused) return;
        rigidbody.velocity = new Vector2(movementData.x * Constants.PLAYER_MOVEMENT_SPEED, rigidbody.velocity.y);
    }

    void Update()
    {
        if (foots.gameObject.activeInHierarchy == true && isDead && isGrounded())
        {
            rigidbody.gravityScale = 0;
            foots.gameObject.SetActive(false);
        }
        if (!IsInitialized || isDead || !canPlay)
        {
            rigidbody.velocity = Vector2.zero;
            return;
        }
        if (gamepad == null) throw new System.NullReferenceException("No gamepad is connected");
        if (gamepad.IsConnected() && !IsConfused)
        {
            ExecuteMovement();
            ExecuteJump();
            flipPlayerAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        Transform collisionTransform = collision.gameObject.transform;

        if (layer == Constants.LAYER_DEADZONE && !GameManager.Instance.IsGameEnded())
            this.OnDeath();

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
            if (collidedPlayer != null && GameManager.Instance.IsGameStarted())
                if (this.canConfuseOtherBirds && !this._isConfused && !collidedPlayer.IsConfused && !collidedPlayer.IsDead())
                {
                    this.SetConfusion(true);
                    collidedPlayer.SetConfusion(true);
                    Vector2 forceDirection;
                    if (this.transform.position.x < collidedPlayer.transform.position.x)
                        forceDirection = new(1, 0.7f);
                    else if (this.transform.position.x > collidedPlayer.transform.position.x)
                        forceDirection = new(-1, 0.7f);
                    else
                    {
                        if (Random.Range(0, 2) == 0)
                            forceDirection = new(1, 0.7f);
                        else
                            forceDirection = new(-1, 0.7f);
                    }
                    this.ApplyForce((Extensions.Vector3.FromVector2(forceDirection).Variation(new(0, 1.2f, 0)) * ForcePower).Opposite(true, false, false));
                    collidedPlayer.ApplyForce(Extensions.Vector3.FromVector2(forceDirection * ForcePower));
                    Instantiate(PrefabStaticStunSphere, this.transform.position.Variation(0, 1.5f, 0), Quaternion.identity);
                }
        }
    }

}
