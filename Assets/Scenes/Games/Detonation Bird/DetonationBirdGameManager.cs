using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonationBirdGameManager : GameManager
{

    public GameObject BombPrefab;
    private List<GameObject> BombSpawns;

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
            p.SetJumpLimit(2);
            p.IgnoreCollisionsWithOtherPlayers(true);
            ((PlatformerPlayer)p).GetHead().GetComponent<Collider2D>().isTrigger = false;
        }
        StartCoroutine(Generation(10, 6, 1));
    }

    IEnumerator Generation(float timeToWait, float bombLoadingTime, float explosionDimension)
    {
        GameObject bomb = Instantiate(BombPrefab, BombSpawns.GetRandom().transform);
        bomb.GetComponent<BombBehaviour>().Initialize(bombLoadingTime, explosionDimension);
        yield return new WaitForSeconds(timeToWait);
        if (!GameManager.Instance.IsGameEnded())
            StartCoroutine(
                Generation(
                    (timeToWait > 1) ? timeToWait - 1f : timeToWait, 
                    (bombLoadingTime > 3) ? bombLoadingTime - 0.5f : bombLoadingTime,
                    (explosionDimension < 5) ? explosionDimension + 0.2f : explosionDimension
                )
            );
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Detonation Bird game");
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
        GameObject[] res = GameObject.FindGameObjectsWithTag("Respawn");
        BombSpawns = new();
        BombSpawns.AddRange(res);
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
