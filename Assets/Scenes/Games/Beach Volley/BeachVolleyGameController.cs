using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachVolleyGameController : GameManager
{
    public override void OnPlayerDies()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerSpawns()
    {
        throw new System.NotImplementedException();
    }

    protected override void FixedUpdateGameSpecificBehaviour()
    {

    }

    protected override List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2: case 4: case 6: case 8:
                return new List<TeamDto>()
                {
                    new TeamDto(){Id = 1},
                    new TeamDto(){Id = 2}
                };
            case 3:
                return new List<TeamDto>()
                {
                    new TeamDto(){Id = 1},
                    new TeamDto(){Id = 2},
                    new TeamDto(){Id = 3}
                };
            default:
                throw new System.ArgumentException("Invalid numberOfPlayers parameter while creating teams");
        }
    }

    protected override void OnRoomStarts()
    {
        foreach(IPlayer player in this.players)
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
        Vector2 generatedForce = Vector2.zero;
        while (generatedForce.x < 0.4f && generatedForce.x > -0.4f)
            generatedForce = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
        Debug.Log(generatedForce);
        rigidbody.AddForce(generatedForce, ForceMode2D.Impulse);
    }

}
