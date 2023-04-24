using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerBirdsFootsteps : PlatformerPlayer
{

    protected override void VariantStart()
    {
        this.canWalk = false;
        this.canJump = false;
    }

    protected override void VariantUpdate()
    {
        if (gamepad.IsConnected())
        {
            if (gamepad.IsButtonPressed(IGamepad.Key.ActionButtonDown, IGamepad.PressureType.Single) && !GameManager.Instance.IsGameEnded()) {
                this.transform.position = new(this.transform.position.x + 0.25f, this.transform.position.y, this.transform.position.z);
                if (GuardianBehaviour.IsWatching)
                    OnDeath();
            }
        }
    }

    protected override void VariantFixedUpdate()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            List<TeamDto> teams = GameManager.Instance.Teams;
            foreach(TeamDto dto in teams)
            {
                if (!dto.players[0].Equals(this))
                    dto.KillAllPlayers();
            }
        }
    }

}
