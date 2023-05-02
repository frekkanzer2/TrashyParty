using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSmashGameManager : GameManager
{

    public Animator WallKillAnimator;

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
            player.SetJumpLimit(10000000);
            PlatformerPlayer pp = (PlatformerPlayer)player;
            player.IgnoreCollisionsWithOtherPlayers(false);
            pp.SetCanKillOtherBirds(true);
        }
        StartCoroutine(StartCounterToWallDown());
    }

    private IEnumerator StartCounterToWallDown()
    {
        yield return new WaitForSeconds(180);
        WallKillAnimator.Play("DOWN");
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Head Smash game");
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
