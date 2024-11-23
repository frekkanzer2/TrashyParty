using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTagGameManager : GameManager
{

    public GameObject BombPrefab;

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
            p.IgnoreCollisionsWithOtherPlayers(false);
            ((PlatformerPlayer)p).GetHead().GetComponent<Collider2D>().isTrigger = false;
            ((PlatformerPlayer)p).SetCanKillOtherBirds(false);
            ((PlatformerPlayer)p).SetCanConfuseOtherBirds(false);
        }
        StartCoroutine(Generation());
    }

    IEnumerator Generation()
    {
        yield return new WaitForSeconds(3);
        if (!this.IsGameEnded())
        {
            PlatformerPlayerBombTag picked = ((PlatformerPlayerBombTag)this.players.FindAll(p => p.IsAlive()).GetRandom());
            GameObject bomb = Instantiate(BombPrefab, picked.transform.position.Variation(0, 2, 0), Quaternion.identity);
            picked.PassBomb(null, bomb);
        }
    }

    public void OnBombDestroyed() => StartCoroutine(Generation());

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Bomb Tag game");
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
