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
            ((PlatformerPlayer)p).SetJumpLimit(2);
            p.IgnoreCollisionsWithOtherPlayers(true);
            ((PlatformerPlayer)p).GetHead().GetComponent<Collider2D>().isTrigger = false;
        }
        StartCoroutine(Generation());
    }

    private int GenerationIteration = 1;
    IEnumerator Generation()
    {
        float timeToWait = 7f - MathfFunction.Logarithmic(GenerationIteration) * 1.6f;
        float bombLoadingTime = 7f - MathfFunction.Logarithmic(GenerationIteration) * 1.15f;
        float explosionDimension = 1.7f + MathfFunction.Logarithmic(GenerationIteration / 1.5f);
        GenerationIteration++;
        GameObject bomb = Instantiate(BombPrefab, BombSpawns.GetRandom().transform);
        bomb.GetComponent<BombBehaviour>().Initialize(bombLoadingTime, explosionDimension);
        yield return new WaitForSeconds(timeToWait);
        if (!GameManager.Instance.IsGameEnded())
            StartCoroutine(Generation());
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
