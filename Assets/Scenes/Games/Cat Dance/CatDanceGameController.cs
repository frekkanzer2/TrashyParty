using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDanceGameController : GameManager
{

    public override void OnPlayerDies()
    {
        base.OnPlayerDies();
    }

    public override void OnPlayerSpawns()
    {

    }

    public override void OnGameEnds()
    {
        base.OnGameEnds();
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
                Id = i + 1
            });
        InitializeTeamMatchVictories(teams);
        SetMatchesVictoryLimit(1);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        foreach (IPlayer player in this.players)
        {
            PlatformerPlayer p = (PlatformerPlayer)player;
            player.IgnoreCollisionsWithOtherPlayers(false);
            p.SetCanKillOtherBirds(false);
            p.SetCanConfuseOtherBirds(true);
            player.SetJumpLimit(3);
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }

    public override void OnPreparationEndsGameSpecific()
    {
        SoundManager.PlayRandomGameSoundtrack();
        GameObject paw = GameObject.Find("CatPaw");
        if (paw == null) throw new System.NullReferenceException("Missing cat paw in the scene");
        paw.GetComponent<CatPawBehaviour>().StartMovement();
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for CatDance game");
    }
}
