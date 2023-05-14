using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRunGameManager : GameManager
{

    public GameObject Missile;
    public GameObject SpawnpointRockets1;
    public GameObject SpawnpointRockets2;
    public GameObject SpawnpointRockets3;
    public GameObject Fireball;

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
        StartCoroutine(MissileGeneration(5, SpawnpointRockets1.transform.position));
        StartCoroutine(MissileGeneration(2.5f, SpawnpointRockets2.transform.position));
        StartCoroutine(MissileGeneration(3.8f, SpawnpointRockets3.transform.position));
        StartCoroutine(FireballGeneration(7f, new(37.95078f, 15.53f, 0)));
        StartCoroutine(FireballGeneration(10f, new(37.95078f, 12.96f, 0)));
    }



    IEnumerator MissileGeneration(float timeToWait, Vector3 pos)
    {
        yield return new WaitForSeconds(timeToWait);
        if (!GameManager.Instance.IsGameEnded())
        {
            Instantiate(Missile, pos, Quaternion.identity);
            StartCoroutine(
                MissileGeneration(
                    timeToWait,
                    pos
                )
            );
        }
    }

    IEnumerator FireballGeneration(float timeToWait, Vector3 pos)
    {
        yield return new WaitForSeconds(timeToWait);
        if (!GameManager.Instance.IsGameEnded())
        {
            Instantiate(Fireball, pos, Quaternion.identity);
            StartCoroutine(
                FireballGeneration(
                    timeToWait,
                    pos
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
