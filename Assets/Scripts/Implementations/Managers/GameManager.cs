using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManager : MonoBehaviour, IGameManager, IMultipleMatchesManager
{
    public GameObject PlayerPrefab;
    public List<GameObject> ActivableFor2Players;
    public List<GameObject> ActivableFor3Players;
    public List<GameObject> ActivableFor4Players;
    public List<GameObject> ActivableFor5Players;
    public List<GameObject> ActivableFor6Players;
    public List<GameObject> ActivableFor7Players;
    public List<GameObject> ActivableFor8Players;
    protected List<IPlayer> players = new List<IPlayer>();
    public List<TeamsForNumberOfPlayers> TeamSpawnpointAssociations;
    protected bool _isGameStarted;
    protected bool _isGameEnded;
    protected ISoundsManager SoundManager;

    private TeamsForNumberOfPlayers TeamSpawnpointAssociationChoise;

    [System.Serializable]
    public class TeamsForNumberOfPlayers
    {
        public int NumberOfPlayersCase;
        public List<TeamRecord> Teams;
        [System.Serializable]
        public class TeamRecord
        {
            public int teamId;
            public List<GameObject> spawnpoints;
        }
    }

    [Header("Monitoring section - do not touch!")]
    public List<TeamDto> Teams;

    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) throw new System.NullReferenceException("Trying to get the Gamepad Manager, but it's not attached as a component!");
            return _instance;
        }
    }

    #region IGameManager implementation

    public void GenerateTeams()
    {
        int players = (int)AppSettings.Get("N_PLAYERS");
        Teams = GenerateTeamsCriteria(players) ?? throw new System.NullReferenceException("No Team is provided for this game!");
        TeamSpawnpointAssociationChoise = TeamSpawnpointAssociations.Find(association => association.NumberOfPlayersCase == players) ?? throw new System.NullReferenceException($"No Team-Spawnpoint association provided for this game and for this number of players ({players})");
        if (Teams.Count == 0) throw new System.NullReferenceException("No Team is provided for this game!");
        else if (Teams.Count != TeamSpawnpointAssociationChoise.Teams.Count) throw new System.NullReferenceException("The number of generated teams via GenerateTeamsCriteria(players) must be equals to the number of teams specified inside the inspector, under the attribute Team Spawnpoint Associations");
        else if (Teams.Find(team => team.players != null) != null) throw new System.ArgumentException("GenerateTeamsCriteria(players) must generate empty teams!");
        for(int i = 1; i <= TeamSpawnpointAssociationChoise.Teams.Count; i++)
        {
            TeamsForNumberOfPlayers.TeamRecord record = TeamSpawnpointAssociationChoise.Teams.Find(team => team.teamId == i);
            TeamDto teamToPopulateWithSpawners = Teams.Find(team => team.Id == i);
            teamToPopulateWithSpawners.spawnpositions = new();
            foreach (GameObject spawnpoint in record.spawnpoints)
                teamToPopulateWithSpawners.spawnpositions.Add(spawnpoint.transform);
        }
    }

    public void ActiveGameObjectsBasedOnPlayerNumber()
    {
        int players = (int)AppSettings.Get("N_PLAYERS");
        List<GameObject> toActivate = null;
        switch(players)
        {
            case 2:
                toActivate = ActivableFor2Players;
                break;
            case 3:
                toActivate = ActivableFor3Players;
                break;
            case 4:
                toActivate = ActivableFor4Players;
                break;
            case 5:
                toActivate = ActivableFor5Players;
                break;
            case 6:
                toActivate = ActivableFor6Players;
                break;
            case 7:
                toActivate = ActivableFor7Players;
                break;
            case 8:
                toActivate = ActivableFor8Players;
                break;
        }
        if (toActivate == null) throw new System.NullReferenceException($"No activation for {players} players: they are not supported!");
        else foreach (GameObject obj in toActivate) obj.SetActive(true);
    }

    public void SpawnPlayers()
    {
        if (Teams.Count == 0) throw new System.ArgumentException("No team was created from minigame! Create at least one team!");
        List<System.Tuple<GameObject, int, int>> playerRecords = new(); // Tuple<colorPrefab, controllerId, PlayerNumber>
        int numberOfPlayers = (int)AppSettings.Get("N_PLAYERS");
        for (int i = 1; i <= numberOfPlayers; i++)
            playerRecords.Add(new((GameObject)AppSettings.Get("COLOR_PLAYER" + i), (int)AppSettings.Get("GAMEPAD_PLAYER" + i), i));
        playerRecords.Shuffle();
        for (int i = 0, teamIndex = 0, teamCycle = 0; i < numberOfPlayers; i++, teamIndex++)
        {
            if (teamIndex == Teams.Count)
            {
                teamIndex = 0;
                teamCycle++;
            }
            TeamDto toPopulate = Teams[teamIndex];
            if (toPopulate.players == null) toPopulate.players = new List<IPlayer>();
            System.Tuple<GameObject, int, int> playerRecord = playerRecords[i];
            GameObject playerGenerated = Instantiate(PlayerPrefab, toPopulate.spawnpositions[teamCycle]);
            playerGenerated.GetComponent<PlayerModel>().ModelPrefab = playerRecord.Item1;
            playerGenerated.GetComponent<IControllerProvider>().ControllerAssociation = new()
            {
                ControllerId = playerRecord.Item2,
                PlayerNumber = playerRecord.Item3
            };
            IPlayer playerComponent = playerGenerated.GetComponent<IPlayer>();
            playerComponent.Id = playerRecord.Item3;
            players.Add(playerComponent);
            toPopulate.players.Add(playerComponent);
        }
    }

    public void OnPreparationEnds()
    {
        foreach (IPlayer p in players)
        {
            p.SetAsReady();
            p.OnGameStarts();
        }
        OnPreparationEndsGameSpecific();
        OnGameStarts();
    }

    public void AssignPoints()
    {
        TeamDto team = this.Teams.Find(t => t.Id == GetTeamIdThatReachedVictoriesLimit());
        RankingDto original = (RankingDto)AppSettings.Get(Constants.APPSETTINGS_RANKING_LABEL);
        RankingDto previous = original.Clone();
        if (team != null)
        {
            List<IPlayer> winners = team.players;
            int[] winnersIds = new int[winners.Count];
            for (int i = 0; i < winners.Count; i++) winnersIds[i] = winners[i].Id;
            original.AddPoint(winnersIds);
            AppSettings.Save(Constants.APPSETTINGS_RANKING_LABEL, original);
            AppSettings.Save(Constants.APPSETTINGS_RANKING_PREVIOUS_LABEL, previous);
        }
        else
            AppSettings.Save(Constants.APPSETTINGS_RANKING_PREVIOUS_LABEL, original);
    }

    #endregion

    private void Awake()
    {
        _instance = this;
        ActiveGameObjectsBasedOnPlayerNumber();
    }

    private void Start()
    {
        SoundManager = SoundsManager.Instance;
        SoundManager.StopAllSounds();
        SoundManager.PlayCountdown();
        GenerateTeams();
        SpawnPlayers();
        _isGameStarted = false;
        _isGameEnded = false;
        OnRoomStarts();
    }

    private void Update()
    {
        UpdateGameSpecificBehaviour();
        if (CanChangeGame) {
            bool pressed = GamepadManager.Instance.IsButtonPressedFromAnyGamepad(IGamepad.Key.Start, IGamepad.PressureType.Single);
            if (pressed)
            {
                CanChangeGame = false;
                AssignPoints();
                GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomEnds();
                StartCoroutine(ExitGameDelayed());
            }
        }
    }

    private IEnumerator ExitGameDelayed()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Ranking", LoadSceneMode.Single);
    }

    private void FixedUpdate()
    {
        FixedUpdateGameSpecificBehaviour();
    }

    #region Game specific methods

    protected abstract void UpdateGameSpecificBehaviour();
    protected abstract void FixedUpdateGameSpecificBehaviour();
    protected abstract void OnRoomStarts();
    protected virtual void OnGameStarts() { if (IsGameStarted()) return; else _isGameStarted = true; }
    public virtual void OnGameEnds() { 
        if (IsGameEnded()) return;
        else
        {
            SoundManager.StopAllSoundsDelayed(1f);
            StartCoroutine(OnGameEndsDelayed());
        }
    }

    protected IEnumerator OnGameEndsDelayed()
    {
        yield return new WaitForSeconds(0.2f);
        if (IsGameEnded()) yield break;
        _isGameEnded = true;
        yield return new WaitForSeconds(1.8f);
        List<TeamDto> aliveTeams = this.Teams.FindAll(t => !t.IsEveryoneDead());
        if (aliveTeams.Count == 1) AddMatchVictory(aliveTeams[0].Id);
        if (GetTeamIdThatReachedVictoriesLimit() != null)
        {
            // the game is definitely ended
            OnEveryMatchEnded();
            yield break;
        }
        try
        {
            RestartMatch(); // it will called if there's a draw or if the victory limit is not reached
        } catch (System.AccessViolationException noAllowedException)
        {
            OnEveryMatchEnded(); // it executes if the game doesn't supports multi-match
        }
    }
    public virtual void OnPlayerDies()
    {
        List<TeamDto> aliveTeams = this.Teams.FindAll(t => !t.IsEveryoneDead());
        if (aliveTeams.Count <= 1)
        {
            foreach (IPlayer p in this.players)
                p.SetAsNotReady();
            OnGameEnds();
        }
    }
    public abstract void OnPlayerSpawns();
    public abstract void OnPreparationEndsGameSpecific();
    protected abstract List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers);

    public bool IsGameEnded() => this._isGameEnded;
    public bool IsGameStarted() => this._isGameStarted;

    #endregion

    #region Multiple matches

    public List<MatchVictoryDto> TeamMatchVictories { get; set; }
    private int victoriesLimit;

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

    public int? GetTeamIdThatReachedVictoriesLimit()
    {
        try
        {
            return this.TeamMatchVictories.Find(t => t.Victories == this.victoriesLimit).TeamId;
        }
        catch (System.NullReferenceException)
        {
            return null;
        }
    }

    public abstract void RestartMatch();

    public void OnEveryMatchEnded()
    {
        StartCoroutine(DisplayExitDelayed());
        SoundManager.PlayEndGameSoundtrack();
        GameObject.FindGameObjectWithTag("Conclusion").GetComponent<Animator>().Play("StartAnimation");
        int? teamWinnerId = GetTeamIdThatReachedVictoriesLimit();
        if (teamWinnerId != null)
        {
            List<GameObject> WinnerDisplayers = new List<GameObject>();
            WinnerDisplayers.AddRange(GameObject.FindGameObjectsWithTag("WinnerDisplayer"));
            TeamDto team = this.Teams.Find(t => t.Id == teamWinnerId);
            
            for (int i = 0; i < team.players.Count; i++)
            {
                IPlayer p = team.players[i];
                Sprite pSprite = p.GetWinSprite();
                WinnerDisplayers[i].GetComponent<SpriteRenderer>().sprite = pSprite;
            }
            if (team.players.Count == 1)
            {
                WinnerDisplayers[0].transform.position = new Vector3(0, -3, 0);
            }
            else if (team.players.Count == 2)
            {
                WinnerDisplayers[0].transform.position = new Vector3(-2.7f, -3, 0);
                WinnerDisplayers[1].transform.position = new Vector3(2.7f, -3, 0);
            }
            else if (team.players.Count == 3)
            {
                WinnerDisplayers[0].transform.position = new Vector3(-5.4f, -3, 0);
                WinnerDisplayers[1].transform.position = new Vector3(0, -3, 0);
                WinnerDisplayers[2].transform.position = new Vector3(5.4f, -3, 0);
            }
            else if (team.players.Count == 4)
            {
                WinnerDisplayers[0].transform.position = new Vector3(-8.1f, -3, 0);
                WinnerDisplayers[1].transform.position = new Vector3(-2.7f, -3, 0);
                WinnerDisplayers[2].transform.position = new Vector3(2.7f, -3, 0);
                WinnerDisplayers[3].transform.position = new Vector3(8.1f, -3, 0);
            }
        }   
    }
    private bool CanChangeGame = false;
    private IEnumerator DisplayExitDelayed()
    {
        yield return new WaitForSeconds(10);
        GameObject continueLabel = GameObject.FindGameObjectWithTag("Conclusion").transform.GetChild(8).gameObject;
        continueLabel.GetComponent<SpriteRenderer>().enabled = true;
        continueLabel.GetComponent<Animator>().enabled = true;
        CanChangeGame = true;
    }

    #endregion

}
