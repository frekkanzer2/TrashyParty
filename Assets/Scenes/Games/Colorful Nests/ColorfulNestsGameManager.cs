using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorfulNestsGameManager : GameManager
{

    public Text timerDisplay;

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
        TimerStarted = true;
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Colorful Nests game");
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
        foreach (IPlayer player in this.players)
        {
            player.IgnoreCollisionsWithOtherPlayers(false);
            PlatformerPlayerColorfulNests p = (PlatformerPlayerColorfulNests)player;
            p.SetCanKillOtherBirds(false);
            p.SetCanConfuseOtherBirds(true);
            p.ChangePlayerStats(Constants.PLAYER_MOVEMENT_SPEED, Constants.PLAYER_JUMPING_POWER + 2);
        }
        TimeLeft = 60;
    }

    public float TimeLeft = 0;
    private bool TimerStarted = false;

    protected override void UpdateGameSpecificBehaviour()
    {
        if (timerDisplay.gameObject.activeInHierarchy && (TimeLeft <= 0 || IsGameEnded() || !IsGameStarted())) timerDisplay.gameObject.SetActive(false);
        else if (TimerStarted && !timerDisplay.gameObject.activeInHierarchy && TimeLeft > 0 && !IsGameEnded()) timerDisplay.gameObject.SetActive(true);
        if (!IsGameEnded() && IsGameStarted() && TimerStarted)
        {
            if (TimeLeft > 0) TimeLeft -= Time.deltaTime;
            else
            {
                RegisterColors();
                int maxColorValue = GetMaxValueColors();
                List<string> winnerNames = GetMaxValueBirdNames(maxColorValue);
                foreach (IPlayer p in this.players)
                    if (!winnerNames.Contains(p.GetName()))
                        p.OnDeath();
                if (players.FindAll(p => p.IsAlive()).Count > 1)
                {
                    Log.Logger.Write(ILogManager.Level.Warning, $"At least 2 birds are alive: restarting timer with 30s");
                    TimeLeft = 30;
                }
            }
        }
        timerDisplay.text = $"{DisplayTimer()}";
    }

    private Dictionary<string, int> records;

    private void RegisterColors()
    {
        records = new();
        foreach (IPlayer p in players)
            records.Add(p.GetName(), 0);
        GameObject[] nests = GameObject.FindGameObjectsWithTag("Pickable");
        foreach (GameObject nest in nests)
        {
            string name = nest.GetComponent<NestBehaviour>().GetAttachedPlayerName();
            if (name is null) continue;
            Log.Logger.Write($"Registering point for bird {name} - Actual: {records[name]} | Next: {records[name]+1}");
            records[name] = records[name] + 1;
        }
    }

    private int GetMaxValueColors()
    {
        return records.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
    }

    private List<string> GetMaxValueBirdNames(int maxValue)
    {
        return records.Where(x => x.Value == maxValue).Select(x => x.Key).ToList();
    }

    private string DisplayTimer()
    {
        float minutes = Mathf.FloorToInt(TimeLeft / 60);
        float seconds = Mathf.FloorToInt(TimeLeft % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds + 1);
    }

}
