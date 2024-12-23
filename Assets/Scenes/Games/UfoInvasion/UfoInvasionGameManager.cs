using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoInvasionGameManager : GameManager
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
        this.gameObject.GetComponent<UfoInvasionGenerator>().StartGeneration();
        foreach (IPlayer player in this.players)
        {
            PlatformerPlayer pp = (PlatformerPlayer)player;
            pp.SetJumpLimit(10000000);
            pp.GetComponent<Rigidbody2D>().gravityScale = 10;
        }
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Ufo Invasion game");
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
            pp.GetHead().GetComponent<CapsuleCollider2D>().isTrigger = false;
            pp.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {
        if (this.IsGameEnded() && this.gameObject.GetComponent<UfoInvasionGenerator>().IsGenerationActive())
            this.gameObject.GetComponent<UfoInvasionGenerator>().EndGeneration();
    }
}
