using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsHatchingGameManager : GameManager
{

    public EggToHitBehaviour eToHit;

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
        eToHit.OnStart();
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Eggs Hatching game");
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
            player.IgnoreCollisionsWithOtherPlayers(false);
            player.SetJumpLimit(1);
            ((PlatformerPlayer)player).GetHead().GetComponent<Collider2D>().isTrigger = true;
            ((PlatformerPlayer)player).SetCanKillOtherBirds(false);
            ((PlatformerPlayer)player).SetCanConfuseOtherBirds(true);
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
