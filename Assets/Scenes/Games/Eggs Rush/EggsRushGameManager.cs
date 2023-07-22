using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggsRushGameManager : GameManager
{

    int startingEggs = 0;
    public List<GameObject> EggSpawners;
    public GameObject EggPrefab;
    private List<GameObject> Eggs;

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
        int increment;
        if (players.Count <= 4) increment = 3;
        else if (players.Count <= 6) increment = 2;
        else increment = 1;
        startingEggs = this.players.Count + increment;
        StartCoroutine(Generate());
    }

    public void OnEggCollided(GameObject collided)
    {
        Eggs.RemoveAll(go => go.name == collided.name);
        if (Eggs.IsEmpty())
        {
            OnRoundEnds();
            StartCoroutine(Generate());
        }
    }

    private void OnRoundEnds()
    {
        if (startingEggs > 1) startingEggs--;
        foreach(IPlayer p in players)
        {
            if (GameManager.Instance.IsGameEnded()) break;
            var castedPlayer = (PlatformerPlayerEggsRush)p;
            if (!castedPlayer.HasPickedEgg) p.OnDeath();
            else castedPlayer.NewRound();
        }
    }

    IEnumerator Generate()
    {
        yield return new WaitForSeconds(3);
        if (!IsGameEnded())
        {
            EggSpawners.Shuffle();
            for (int i = 0; i < startingEggs; i++)
            {
                Vector3 spawnPosition = EggSpawners[i].transform.position;
                GameObject egg = Instantiate(EggPrefab, spawnPosition, Quaternion.identity);
                egg.name = "SpawnedEgg" + i;
                Eggs.Add(egg);
            }
        }
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Eggs Rush game");
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
        Eggs = new();
        foreach (IPlayer player in this.players)
        {
            player.IgnoreCollisionsWithOtherPlayers(false);
            ((PlatformerPlayer)player).SetJumpLimit(2);
            ((PlatformerPlayer)player).GetHead().GetComponent<Collider2D>().isTrigger = true;
            ((PlatformerPlayer)player).SetCanKillOtherBirds(false);
            ((PlatformerPlayer)player).SetCanConfuseOtherBirds(true);
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
