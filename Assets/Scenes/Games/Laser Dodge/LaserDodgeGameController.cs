using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDodgeGameController : GameManager
{

    public GameObject limiter, background;
    public Sprite[] limiters, backgrounds;
    public GameObject LaserPrefab;

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
            p.SetCanSprint(false);
            p.ChangePlayerStats(Constants.PLAYER_MOVEMENT_SPEED - 3, 0);
            p.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 1);
        }
        // TESTING LASER
        ILaser laser = (Instantiate(LaserPrefab, new Vector3(0, 0, 0), Quaternion.identity)).GetComponent<ILaser>();
        laser.Initialize(true, 10, 20, 5, 0.05f, 0.75f);
        laser.OnSpawn();
    }

    float _timer = 0;
    int _inttimer = 0, _spriteChoise = 0;
    protected override void UpdateGameSpecificBehaviour()
    {
        _timer += Time.deltaTime;
        _inttimer = (int)(_timer % 60);
        _spriteChoise = _inttimer % 2;
        background.GetComponent<SpriteRenderer>().sprite = backgrounds[_spriteChoise];
        limiter.GetComponent<SpriteRenderer>().sprite = limiters[_spriteChoise];
    }

    public override void OnPreparationEndsGameSpecific()
    {
        SoundManager.PlayRandomGameSoundtrack();
    }

    public override void RestartMatch()
    {
        this._isGameEnded = false;
        this._isGameStarted = false;
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
