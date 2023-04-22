using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDodgeGameManager : GameManager
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
        this.gameObject.GetComponent<LavaBallGenerator>().StartGeneration();
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Lava Dodge game");
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
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {
        if (this.IsGameEnded() && this.gameObject.GetComponent<LavaBallGenerator>().IsGenerationActive())
            this.gameObject.GetComponent<LavaBallGenerator>().EndGeneration();
    }
}
