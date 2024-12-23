using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullShitGameManager : GameManager
{

    public BirdyBullBehaviour Bull;

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
        Bull.StartBehaviour();
    }

    public override void RestartMatch() => throw new System.AccessViolationException("No restart is allowed for Bull Shit game");

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
        foreach (IPlayer player in this.players) {
            player.IgnoreCollisionsWithOtherPlayers(false);
            TopDownPlayer p = (TopDownPlayer)player;
            p.SetCanSprint(false);
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
