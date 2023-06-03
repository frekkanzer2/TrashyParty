using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderTheRainGameManager : GameManager
{

    public GameObject Missile;

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
            p.IgnoreCollisionsWithOtherPlayers(true);
            ((PlatformerPlayer)p).GetHead().GetComponent<Collider2D>().isTrigger = false;
        }
        StartCoroutine(Generation(38f));
    }

    private int GenerationIteration = 1;
    IEnumerator Generation(float rangeValue)
    {
        Instantiate(Missile, new Vector3(Random.Range(-1 * rangeValue, rangeValue), 26, 0), Quaternion.identity);
        float timeToWait; 
        if (GenerationIteration <= 60) timeToWait = 3.4f - MathfFunction.SquareRoot(GenerationIteration) / 2.5f;
        else timeToWait = 0.63f - MathfFunction.SquareRoot(30 + GenerationIteration - 60) / 17f;
        if (timeToWait < 0.15f) timeToWait = 0.15f;
        yield return new WaitForSeconds(timeToWait);
        GenerationIteration++;
        if (!GameManager.Instance.IsGameEnded())
        {
            StartCoroutine(
                Generation(
                    (rangeValue > 24f) ? rangeValue - 0.25f : rangeValue
                )
            );
        }
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

    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
