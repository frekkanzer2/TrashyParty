using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GCEAGameManager : GameManager
{

    public Text timerDisplay;
    public GameObject middleSpawnpointToMove;

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
        StartCoroutine(StartCatching());
        foreach(TeamDto t in this.Teams) t.players = new();
        this.players.Shuffle();
        this.Teams.Find(t => t.Id == 1).players.Add(this.players[0]);
        for (int i = 1; i < this.players.Count; i++)
            this.Teams.Find(t => t.Id == 2).players.Add(this.players[i]);
    }

    IEnumerator StartCatching()
    {
        yield return new WaitForSeconds(3);
        foreach (IPlayer p in this.Teams.Find(t => t.Id == 1).players) ((PlatformerPlayerGCEA)p).TurnAsCatcher();
        TimerStarted = true;
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for GCEA game");
    }

    protected override void FixedUpdateGameSpecificBehaviour()
    {

    }

    protected override List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers)
    {
        List<TeamDto> teams = new List<TeamDto>() {
            new() { Id = 1 }, // catcher id
            new() { Id = 2 }
        };
        InitializeTeamMatchVictories(teams);
        SetMatchesVictoryLimit(1);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        if (this.players.Count == 8)
            middleSpawnpointToMove.transform.position = new(-2.94f, 2.125f, 0.1820703f);
        foreach (IPlayer player in this.players)
        {
            player.SetJumpLimit(2);
            player.IgnoreCollisionsWithOtherPlayers(false);
            ((PlatformerPlayerGCEA)player).SetCanKillOtherBirds(false);
            ((PlatformerPlayerGCEA)player).SetCanConfuseOtherBirds(false);
            ((PlatformerPlayer)player).GetHead().GetComponent<Collider2D>().isTrigger = false;
        }
        if (this.players.Count == 2) TimeLeft = 45;
        else if (this.players.Count <= 4) TimeLeft = 80;
        else if (this.players.Count <= 6) TimeLeft = 120;
        else if (this.players.Count <= 8) TimeLeft = 180;
    }

    private float TimeLeft = 0;
    private bool TimerStarted = false;

    protected override void UpdateGameSpecificBehaviour()
    {
        if (timerDisplay.gameObject.activeInHierarchy && (TimeLeft <= 0 || IsGameEnded() || !IsGameStarted())) timerDisplay.gameObject.SetActive(false);
        else if (TimerStarted && !timerDisplay.gameObject.activeInHierarchy && TimeLeft > 0 && !IsGameEnded()) timerDisplay.gameObject.SetActive(true);
        if (!IsGameEnded() && IsGameStarted() && TimerStarted)
        {
            if (TimeLeft > 0) TimeLeft -= Time.deltaTime;
            else if (this.Teams.Find(t => t.Id == 2).GetAlivePlayers().Count > 0) this.Teams.Find(t => t.Id == 1).KillAllPlayers();
        }
        timerDisplay.text = $"{DisplayTimer()}";
    }

    private string DisplayTimer()
    {
        float minutes = Mathf.FloorToInt(TimeLeft / 60);
        float seconds = Mathf.FloorToInt(TimeLeft % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds+1);
    }
}
