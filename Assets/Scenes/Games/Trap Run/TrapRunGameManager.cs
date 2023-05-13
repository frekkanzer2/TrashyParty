using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRunGameManager : GameManager
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
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Under the Rain game");
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
        {
            p.IgnoreCollisionsWithOtherPlayers(false);
            ((PlatformerPlayer)p).GetHead().GetComponent<Collider2D>().isTrigger = true;
            ((PlatformerPlayer)p).SetCanKillOtherBirds(false);
            ((PlatformerPlayer)p).SetCanConfuseOtherBirds(true);
            ((PlatformerPlayer)p).ChangePlayerStats(Constants.PLAYER_MOVEMENT_SPEED - 1.5f, Constants.PLAYER_JUMPING_POWER+1);
            ((PlatformerPlayer)p).transform.localScale = new Vector3(0.85f, 0.85f, 1);
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
