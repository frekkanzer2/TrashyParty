using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyDropGameManager : GameManager
{

    public PathBehaviour path;

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
            player.SetJumpLimit(2);
            player.IgnoreCollisionsWithOtherPlayers(true);
            ((PlatformerPlayer)player).GetHead().GetComponent<Collider2D>().isTrigger = false;
        }
        path.StartPath();
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Deadly Drop game");
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

    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
