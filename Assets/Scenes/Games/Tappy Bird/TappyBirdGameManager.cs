using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TappyBirdGameManager : GameManager
{

    public GameObject little, medium, big;
    private int level = 0, instance = 0;

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
            player.SetCanWalk(false);
            PlatformerPlayer pp = (PlatformerPlayer)player;
            pp.GetComponent<Rigidbody2D>().gravityScale = 14;
        }
        this.level = 1;
        this.instance = 0;
        StartCoroutine(Generate(2));
    }

    IEnumerator Generate(float waitingTime)
    {
        Vector3 spawnPosition = new Vector3(42, Random.Range(-12.5f, 12.5f), 0);
        GameObject generated = null;
        switch(level)
        {
            case 1:
                generated = Instantiate(little, spawnPosition, Quaternion.identity);
                break;
            case 2:
                generated = Instantiate(medium, spawnPosition, Quaternion.identity);
                break;
            case 3:
                generated = Instantiate(big, spawnPosition, Quaternion.identity);
                break;
        }
        if (!GameManager.Instance.IsGameEnded())
        {
            instance++;
            if (instance == 10) level = 2;
            if (instance == 20) level = 3;
        }
        yield return new WaitForSeconds(waitingTime);
        if (instance > 30 && waitingTime > 1) waitingTime -= 0.05f;
        if (!GameManager.Instance.IsGameEnded()) StartCoroutine(Generate(waitingTime));
    }

    public override void RestartMatch()
    {
        this._isGameEnded = false;
        this._isGameStarted = false;
        instance = 0;
        level = 0;
        GameObject[] tubes = GameObject.FindGameObjectsWithTag("Finish");
        foreach (GameObject tube in tubes)
            Destroy(tube);
        GameObject presentation = GameObject.FindGameObjectWithTag("Presentation");
        presentation.GetComponent<SpriteRenderer>().enabled = true;
        presentation.GetComponent<Animator>().enabled = true;
        presentation.GetComponent<Animator>().Play(Constants.ANIMATION_PRESENTATION_STATE);
        foreach (IPlayer p in players)
        {
            p.SetAsNotReady();
            p.OnSpawn();
            PlatformerPlayer pp = (PlatformerPlayer)p;
            pp.GetComponent<Rigidbody2D>().gravityScale = 0;
            pp.gameObject.transform.position = p.RespawnPosition;
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
        SetMatchesVictoryLimit(2);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        foreach (IPlayer player in this.players)
        {
            player.IgnoreCollisionsWithOtherPlayers(true);
            PlatformerPlayer pp = (PlatformerPlayer)player;
            pp.GetHead().GetComponent<CapsuleCollider2D>().isTrigger = false;
            player.RespawnPosition = pp.gameObject.transform.position;
            pp.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
