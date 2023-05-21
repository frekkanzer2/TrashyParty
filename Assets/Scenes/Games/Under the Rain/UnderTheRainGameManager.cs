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
        StartCoroutine(Generation(3, 38f));
    }

    IEnumerator Generation(float timeToWait, float rangeValue)
    {
        Instantiate(Missile, new Vector3(Random.Range(-1 * rangeValue, rangeValue), 26, 0), Quaternion.identity);
        yield return new WaitForSeconds(timeToWait);
        if (!GameManager.Instance.IsGameEnded())
        {
            if (timeToWait > 1 && rangeValue > 25f) timeToWait -= 0.15f;
            else if (timeToWait > 0.15f && rangeValue < 25f) timeToWait -= 0.005f;
            StartCoroutine(
                Generation(
                    timeToWait,
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
