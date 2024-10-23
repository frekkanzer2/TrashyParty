using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingSeasonGameManager : GameManager
{

    public GameObject hunter_prefab;
    public Vector3 yhunterpos;
    
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
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed");
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
        foreach (IPlayer player in players)
        {
            player.IgnoreCollisionsWithOtherPlayers(true);
            player.SetCanJump(true);
            player.SetCanWalk(false);
            player.SetJumpLimit(1);
        }

        for (int i = 1; i<players.Count+1; i++)
        {
            Instantiate(hunter_prefab, yhunterpos, Quaternion.identity) ;
            yhunterpos.y = yhunterpos.y - 3.81f;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
