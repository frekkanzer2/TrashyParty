using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachVolleyGameController : GameManager, IMultipleMatchesManager
{
    public List<MatchVictoryDto> TeamMatchVictories { get; set; }
    private int victoriesLimit;

    public override void OnPlayerDies()
    {
        List<TeamDto> aliveTeams = this.Teams.FindAll(t => !t.IsEveryoneDead());
        if (aliveTeams.Count <= 1) OnGameEnds();
    }

    public override void OnPlayerSpawns()
    {

    }

    public override void OnGameEnds()
    {
        base.OnGameEnds();
        StartCoroutine(OnGameEndsDelayed());
    }

    private IEnumerator OnGameEndsDelayed()
    {
        yield return new WaitForSeconds(1);
        List<TeamDto> aliveTeams = this.Teams.FindAll(t => !t.IsEveryoneDead());
        if (aliveTeams.Count == 1)
        {
            // a team wons!
            TeamDto winnerTeam = aliveTeams[0];
            AddMatchVictory(winnerTeam.Id);
            if (GetTeamIdThatReachedVictoriesLimit() != null)
            {
                // the game is definitely ended
                OnEveryMatchEnded();
                yield break;
            }
        }
        RestartMatch(); // it will called if there's a draw or if the victory limit is not reached
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
            case 3:
                teams = new List<TeamDto>()
                {
                    new TeamDto(){Id = 1},
                    new TeamDto(){Id = 2},
                    new TeamDto(){Id = 3}
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
            p.GetHead().GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }

    public override void OnPreparationEndsGameSpecific()
    {
        GameObject ball = GameObject.Find("BeachVolleyBall");
        if (ball == null) throw new System.NullReferenceException("Missing volley ball in the scene");
        Rigidbody2D rigidbody = ball.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 1;
        Vector2 generatedForce = Vector2.zero;
        while (generatedForce.x < 0.4f && generatedForce.x > -0.4f)
            generatedForce = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
        rigidbody.AddForce(generatedForce, ForceMode2D.Impulse);
    }

    public void InitializeTeamMatchVictories(List<TeamDto> teams)
    {
        TeamMatchVictories = new();
        foreach (TeamDto t in teams)
            TeamMatchVictories.Add(new MatchVictoryDto()
            {
                TeamId = t.Id,
                Victories = 0
            });
    }

    public void SetMatchesVictoryLimit(int limit) => victoriesLimit = limit;

    public void AddMatchVictory(int winnerTeamId) => this.TeamMatchVictories.Find(t => t.TeamId == winnerTeamId).Victories += 1;

    public int? GetTeamIdThatReachedVictoriesLimit() {
        try
        {
            return this.TeamMatchVictories.Find(t => t.Victories == this.victoriesLimit).TeamId;
        } catch (System.NullReferenceException)
        {
            return null;
        }
    }

    public void OnEveryMatchEnded()
    {
        throw new System.NotImplementedException($"EVERY MATCH IS ENDED! THE WINNER IS TEAM {GetTeamIdThatReachedVictoriesLimit()}");
    }

    public void RestartMatch()
    {
        this._isGameEnded = false;
        this._isGameStarted = false;
        GameObject ball = GameObject.Find("BeachVolleyBall");
        if (ball == null) throw new System.NullReferenceException("Missing volley ball in the scene");
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
        }
    }
}
