using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSmashGameManager : GameManager
{

    public GameObject UpperGroundPrefab;
    private GameObject UpperGroundInstance;

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
            player.RespawnPosition = pp.gameObject.transform.position;
        }
    }

    public override void RestartMatch()
    {
        this._isGameEnded = false;
        this._isGameStarted = false;
        Destroy(this.UpperGroundInstance);
        this.UpperGroundInstance = Instantiate(this.UpperGroundPrefab, new(0, 37.42f, 0), Quaternion.identity);
        GameObject presentation = GameObject.FindGameObjectWithTag("Presentation");
        presentation.GetComponent<SpriteRenderer>().enabled = true;
        presentation.GetComponent<Animator>().enabled = true;
        presentation.GetComponent<Animator>().Play(Constants.ANIMATION_PRESENTATION_STATE);
        foreach (IPlayer p in players)
        {
            p.SetAsNotReady();
            p.OnSpawn();
            ((PlatformerPlayer)p).transform.position = p.RespawnPosition;
        }
        SoundManager.PlayCountdown();
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
        SetMatchesVictoryLimit(3);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        this.UpperGroundInstance = Instantiate(this.UpperGroundPrefab, new(0, 37.42f, 0), Quaternion.identity);
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
