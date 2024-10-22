using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CannonEggGameManager : GameManager
{
    private List<Cannon> Cannons;
    public override void OnPlayerDies()
    {
        base.OnPlayerDies();
    }

    public override void OnPlayerSpawns()
    {

    }
    public override void OnPreparationEndsGameSpecific()
    {
        Cannons = new();
        Cannons = GameObject.FindGameObjectsWithTag("Respawn").ToList().Select(item => item.GetComponent<Cannon>()).ToList();
        SoundManager.PlayRandomGameSoundtrack();
        StartCoroutine(Generation());
    }

    private int GenerationIteration = 1;

    IEnumerator Generation()
    {
        Cannon shooter = Cannons.Where(c => c.CanShoot).ToList().GetRandom();
        shooter.Shoot();
        float timeToWait;
        if (GenerationIteration <= 45) timeToWait = 3f - MathfFunction.SquareRoot(((float)GenerationIteration)) / (3.35f - ((float)GenerationIteration)/50f);
        else timeToWait = 0.25f;
        yield return new WaitForSeconds(timeToWait);
        GenerationIteration++;
        if (!GameManager.Instance.IsGameEnded()) StartCoroutine(Generation());
    }
    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed");
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
                Id = i + 1
            });
        InitializeTeamMatchVictories(teams);
        SetMatchesVictoryLimit(1);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        foreach (IPlayer player in this.players)
        {
            TopDownPlayer p = (TopDownPlayer)player;
            player.IgnoreCollisionsWithOtherPlayers(false);
            p.ChangeSprintBarRecoveryValue(0.05f);
            p.ChangeSprintBarSprintConsumingValue(0.5f);
            p.gameObject.transform.localScale = new Vector3(1.65f, 1.65f, 1);
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
