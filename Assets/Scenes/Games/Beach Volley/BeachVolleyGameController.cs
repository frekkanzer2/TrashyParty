using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachVolleyGameController : GameManager
{
    public override void OnPlayerDies()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerSpawns()
    {
        throw new System.NotImplementedException();
    }

    protected override void FixedUpdateGameSpecificBehaviour()
    {

    }

    protected override List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2: case 4: case 6: case 8:
                return new List<TeamDto>()
                {
                    new TeamDto(){Id = 1},
                    new TeamDto(){Id = 2}
                };
            case 3:
                return new List<TeamDto>()
                {
                    new TeamDto(){Id = 1},
                    new TeamDto(){Id = 2},
                    new TeamDto(){Id = 3}
                };
            default:
                throw new System.ArgumentException("Invalid numberOfPlayers parameter while creating teams");
        }
    }

    protected override void OnRoomStarts()
    {
        foreach(IPlayer player in this.players)
        {
            PlatformerPlayer p = (PlatformerPlayer)player;
            p.GetHead().GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
