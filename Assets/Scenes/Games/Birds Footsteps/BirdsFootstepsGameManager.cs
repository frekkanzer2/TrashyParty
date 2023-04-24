using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsFootstepsGameManager : GameManager
{

    public GuardianBehaviour guardian;
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
        guardian.StartWatch();
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
        foreach (IPlayer p in players)
            ((PlatformerPlayerBirdsFootsteps)p).gameObject.transform.localScale = new Vector3(2, 2, 1);
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
