using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketeggGameController : GameManager
{

    public override void OnPlayerDies()
    {
        base.OnPlayerDies();
    }

    public override void OnPlayerSpawns()
    {

    }

    public override void OnGameEnds()
    {
        base.OnGameEnds();
    }

    protected override void FixedUpdateGameSpecificBehaviour()
    {

    }

    protected override List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers)
    {
        List<TeamDto> teams;
        switch (numberOfPlayers)
        {
            case 2: case 4: case 6: case 8:
                teams = new List<TeamDto>()
                {
                    new TeamDto(){Id = 1},
                    new TeamDto(){Id = 2}
                };
                break;
            default:
                throw new System.ArgumentException("Invalid numberOfPlayers parameter while creating teams");
        }
        InitializeTeamMatchVictories(teams);
        SetMatchesVictoryLimit(3);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        foreach (IPlayer player in this.players)
        {
            PlatformerPlayer p = (PlatformerPlayer)player;
            player.IgnoreCollisionsWithOtherPlayers(false);
            p.SetCanKillOtherBirds(false);
            p.SetCanConfuseOtherBirds(true);
            p.SetJumpLimit(5);
            player.RespawnPosition = p.gameObject.transform.position;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }

    public override void OnPreparationEndsGameSpecific()
    {
        SoundManager.PlayRandomGameSoundtrack();
        GameObject ball = GameObject.Find("BasketEgg");
        if (ball == null) throw new System.NullReferenceException("Missing egg in the scene");
        Rigidbody2D rigidbody = ball.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 3;
        Vector2 generatedForce = Vector2.zero;
        while (generatedForce.x < 0.4f && generatedForce.x > -0.4f)
            generatedForce = new Vector2(Random.Range(-2f, 2f), Random.Range(0.5f, 1f));
        rigidbody.AddForce(generatedForce, ForceMode2D.Impulse);
    }

    public override void RestartMatch()
    {
        this._isGameEnded = false;
        this._isGameStarted = false;
        GameObject ball = GameObject.Find("BasketEgg");
        if (ball == null) throw new System.NullReferenceException("Missing egg in the scene");
        Rigidbody2D rigidbody = ball.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        rigidbody.velocity = Vector2.zero;
        ball.transform.position = new Vector3(0, 8, 0);
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
}
