using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyBoxesGameManager : GameManager
{
    public override void OnPlayerDies()
    {
        base.OnPlayerDies();
    }

    public override void OnPlayerSpawns()
    {

    }

    public override void OnPreparationEndsGameSpecific()
    {
        SoundManager.PlayRandomGameSoundtrack();
        foreach (IPlayer player in this.players)
        {
            player.SetJumpLimit(0);
            PlatformerPlayer pp = (PlatformerPlayer)player;
            pp.GetComponent<Rigidbody2D>().gravityScale = 14;
        }
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Cloudy Boxes game");
    }

    protected override void FixedUpdateGameSpecificBehaviour()
    {

    }

    protected override List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers)
    {
        List<TeamDto> teams = new List<TeamDto>();
        for (int i = 0; i < numberOfPlayers; i++)
            teams.Add(new()
            {
                Id = i+1
            });
        InitializeTeamMatchVictories(teams);
        SetMatchesVictoryLimit(1);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        foreach (IPlayer player in this.players)
        {
            player.IgnoreCollisionsWithOtherPlayers(true);
            PlatformerPlayer pp = (PlatformerPlayer)player;
            pp.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
