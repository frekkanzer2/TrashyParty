using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : IPlayer
{
    public bool IsInitialized => true;

    public Vector3 RespawnPosition { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void ApplyForce(Vector2 force)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyForce(Vector2 force, float countdownInSeconds)
    {
        throw new System.NotImplementedException();
    }

    public Sprite GetWinSprite()
    {
        throw new System.NotImplementedException();
    }

    public void IgnoreCollisionsWithOtherPlayers(bool active)
    {
    }

    public void Initialize()
    {
    }
    private bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }

    public void OnDeath()
    {
        isDead = true;
        GameManager.Instance.OnPlayerDies();
    }

    public void OnGameEnds()
    {
    }

    public void OnGameStarts()
    {
    }

    public void OnSpawn()
    {
        throw new System.NotImplementedException();
    }

    public void SetAsNotReady()
    {
    }

    public void SetAsReady()
    {
    }

    public void SetCanJump(bool b)
    {
    }

    public void SetCanWalk(bool b)
    {
    }

    public void SetGamepad(IGamepad gamepad)
    {
        throw new System.NotImplementedException();
    }

    public void SetGamepadByAssociation(PlayerControllerAssociationDto pcaDto)
    {
        throw new System.NotImplementedException();
    }

    public void SetJumpLimit(int limit)
    {
    }
}
