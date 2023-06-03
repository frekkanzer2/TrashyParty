using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagomeGameManager : GameManager
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
        foreach (IPlayer p in players)
        {
            p.SetCanWalk(false);
            p.SetCanJump(false);
        }
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Kagome game");
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
        TeamDto dummyTeam = new(){Id = 100};
        this.Teams.Add(dummyTeam);
        dummyTeam.players = new();
        dummyTeam.players.Add(new DummyPlayer());
        Log.Logger.Write("Added dummy player");
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
    
}
