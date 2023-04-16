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
    protected List<GameObject> PlayerSpawnpoints = new List<GameObject>();
    private bool isGameStarted;
    private bool isGameEnded;

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
        else foreach (GameObject obj in toActivate)
            {
                obj.SetActive(true);
                if (obj.transform.childCount > 0)
                    foreach (Transform child in obj.transform)
                        if (child.gameObject.CompareTag("Spawnpoint"))
                            PlayerSpawnpoints.Add(child.gameObject);
            }
    }

    public void SpawnPlayers()
    {
        for(int i = 1; i <= (int)AppSettings.Get("N_PLAYERS"); i++)
        {
            GameObject colorPrefab = (GameObject)AppSettings.Get("COLOR_PLAYER" + i);
            int controllerId = (int)AppSettings.Get("GAMEPAD_PLAYER" + i);
            GameObject playerGenerated = Instantiate(PlayerPrefab, PlayerSpawnpoints[i - 1].transform);
            playerGenerated.GetComponent<PlayerModel>().ModelPrefab = colorPrefab;
            playerGenerated.GetComponent<IControllerProvider>().ControllerAssociation = new()
            {
                ControllerId = controllerId,
                PlayerNumber = i
            };
            players.Add(playerGenerated.GetComponent<IPlayer>());
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

    #endregion

}
