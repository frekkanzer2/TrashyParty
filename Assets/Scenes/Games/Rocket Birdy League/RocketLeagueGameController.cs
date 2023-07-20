using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLeagueGameController : GameManager
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
            TopDownPlayer p = (TopDownPlayer)player;
            player.IgnoreCollisionsWithOtherPlayers(false);
            player.RespawnPosition = p.gameObject.transform.position;
            p.ChangePlayerStats(Constants.PLAYER_MOVEMENT_SPEED - 5, Constants.PLAYER_SPRINT_MOVEMENT_SPEED - 4);
            p.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 1);
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }

    public override void OnPreparationEndsGameSpecific()
    {
        SoundManager.PlayRandomGameSoundtrack();
    }

    public override void RestartMatch()
    {
        this._isGameEnded = false;
        this._isGameStarted = false;
        GameObject ball = GameObject.Find("TopDownSoccerBall");
        if (ball == null) throw new System.NullReferenceException("Missing soccer ball in the scene");
        Rigidbody2D rigidbody = ball.GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;
        ball.transform.position = new Vector3(0, 0, 0);
        GameObject presentation = GameObject.FindGameObjectWithTag("Presentation");
        presentation.GetComponent<SpriteRenderer>().enabled = true;
        presentation.GetComponent<Animator>().enabled = true;
        presentation.GetComponent<Animator>().Play(Constants.ANIMATION_PRESENTATION_STATE);
        foreach (IPlayer p in players)
        {
            p.SetAsNotReady();
            p.OnSpawn();
            ((TopDownPlayer)p).transform.position = p.RespawnPosition;
        }
        SoundManager.PlayCountdown();
    }
}
