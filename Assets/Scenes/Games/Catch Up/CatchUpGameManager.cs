using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchUpGameManager : GameManager
{

    public GameObject Camera;
    public GameObject Winner1;

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
            p.IgnoreCollisionsWithOtherPlayers(false);
            ((PlatformerPlayer)p).SetCanKillOtherBirds(false);
            ((PlatformerPlayer)p).SetCanConfuseOtherBirds(true);
            ((PlatformerPlayer)p).ChangePlayerStats(Constants.PLAYER_MOVEMENT_SPEED - 2, Constants.PLAYER_JUMPING_POWER - 8);
        }
        Camera.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0), ForceMode2D.Force);
        StartCoroutine(AddMoreForceToCamera(2, 25));
        StartCoroutine(AddMoreForceToCamera(4, 25));
        StartCoroutine(AddMoreForceToCamera(6, 50));
        StartCoroutine(AddMoreForceToCamera(10, 50));
        StartCoroutine(AddMoreForceToCamera(12, 50));
        StartCoroutine(AddMoreForceToCamera(14, 50));
        StartCoroutine(AddMoreForceToCamera(16, 50));
        StartCoroutine(AddMoreForceToCamera(18, 50));
        StartCoroutine(AddMoreForceToCamera(20, 50));
    }

    IEnumerator AddMoreForceToCamera(float awaitSeconds, float force)
    {
        yield return new WaitForSeconds(awaitSeconds);
        Camera.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0), ForceMode2D.Force);
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Catch Up game");
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
            ((PlatformerPlayer)p).gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 1);
    }

    private bool changedVelocityEnds = false;
    protected override void UpdateGameSpecificBehaviour()
    {
        if (IsGameEnded())
        {
            if (!changedVelocityEnds)
            {
                changedVelocityEnds = true;
                Camera.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Camera.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0), ForceMode2D.Force);
            }
            Winner1.transform.localPosition = new(0, Winner1.transform.localPosition.y, 0);
            if (Camera.transform.position.x >= 396.5f)
                Camera.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
