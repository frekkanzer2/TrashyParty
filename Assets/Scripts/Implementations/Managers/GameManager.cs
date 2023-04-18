using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour, IGameManager
{
    public GameObject PlayerPrefab;
    public List<Sprite> Presentation;
    public List<GameObject> ActivableFor2Players;
    public List<GameObject> ActivableFor3Players;
    public List<GameObject> ActivableFor4Players;
    public List<GameObject> ActivableFor5Players;
    public List<GameObject> ActivableFor6Players;
    public List<GameObject> ActivableFor7Players;
    public List<GameObject> ActivableFor8Players;
    protected List<IPlayer> players = new List<IPlayer>();
    public List<TeamsForNumberOfPlayers> TeamSpawnpointAssociations;
    private bool isGameStarted;
    private bool isGameEnded;

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
            toPopulate.players = new List<IPlayer>();
            System.Tuple<GameObject, int, int> playerRecord = playerRecords[i];
            GameObject playerGenerated = Instantiate(PlayerPrefab, toPopulate.spawnpositions[teamCycle]);
            playerGenerated.GetComponent<PlayerModel>().ModelPrefab = playerRecord.Item1;
            playerGenerated.GetComponent<IControllerProvider>().ControllerAssociation = new()
            {
                ControllerId = playerRecord.Item2,
                PlayerNumber = playerRecord.Item3
            };
            players.Add(playerGenerated.GetComponent<IPlayer>());
            toPopulate.players.Add(playerGenerated.GetComponent<IPlayer>());
        }
    }

    public void StartPreparationAnimation()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    private void Awake()
    {
        _instance = this;
        ActiveGameObjectsBasedOnPlayerNumber();
    }

    private void Start()
    {
        GenerateTeams();
        SpawnPlayers();
        isGameStarted = false;
        isGameEnded = false;
        StartPreparationAnimation();
        OnRoomStarts();
    }

    private void Update()
    {
        UpdateGameSpecificBehaviour();
    }

    private void FixedUpdate()
    {
        FixedUpdateGameSpecificBehaviour();
    }

    #region Game specific methods

    protected abstract void UpdateGameSpecificBehaviour();
    protected abstract void FixedUpdateGameSpecificBehaviour();
    protected abstract void OnRoomStarts();
    protected virtual void OnGameStarts() { if (isGameStarted) return; else isGameStarted = true; }
    protected virtual void OnGameEnds() { if (isGameEnded) return; else isGameEnded = true; }
    public abstract void OnPlayerDies();
    public abstract void OnPlayerSpawns();
    protected abstract List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers);

    #endregion

}
