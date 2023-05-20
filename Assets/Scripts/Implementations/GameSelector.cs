using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameSelector : MonoBehaviour, IGameSelector
{

    public List<Constants.GameName> FilterGamesByPlayersNumber(int numberOfPlayers)
    {
        List<Constants.GameName> playables = new();
        foreach (int i in System.Enum.GetValues(typeof(Constants.GameName)))
        {
            if (IsGamePossibleByPlayersNumber((Constants.GameName)i, numberOfPlayers))
                playables.Add((Constants.GameName)i);
        }
        return playables;
    }

    public bool IsGamePossibleByPlayersNumber(Constants.GameName name, int numberOfPlayers)
    {
        int[] supportedPlayers;
        switch (name)
        {
            case Constants.GameName.BeachVolley:
                supportedPlayers = new int[] { 2, 3, 4, 6, 8 };
                break;
            case Constants.GameName.CloudyBoxes:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.LavaDodge:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.EnergyRelease:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.BirdsFootsteps:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7 };
                break;
            case Constants.GameName.Electroshock:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.HeadSmash:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.DeadlyDrop:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.TappyBird:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.UfoInvasion:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.ImTheKing:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6 };
                break;
            case Constants.GameName.BirdSoccer:
                supportedPlayers = new int[] { 2, 4, 6, 8 };
                break;
            case Constants.GameName.CatDance:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.Basketegg:
                supportedPlayers = new int[] { 2, 4, 6, 8 };
                break;
            case Constants.GameName.LaboratoryLights:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.DetonationBird:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.FizzleFloor:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.UnderTheRain:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.CatchUp:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.BombTag:
                supportedPlayers = new int[] { 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.SuperHot:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.TrapRun:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.StaticStun:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.EggsRush:
                supportedPlayers = new int[] { 3, 4, 5, 6, 7, 8 };
                break;
            case Constants.GameName.GCEA:
                supportedPlayers = new int[] { 2, 3, 4, 5, 6, 7, 8 };
                break;
            default:
                throw new System.NullReferenceException("Missing game inside the game selection");
        }
        if (!supportedPlayers.Contains(numberOfPlayers)) return false;
        return true;
    }

    public void SelectGame(Constants.GameName game, int numberOfPlayers)
    {
        if (!IsGamePossibleByPlayersNumber(game, numberOfPlayers))
            throw new System.ArgumentException($"The game {game} doesn't supports {numberOfPlayers} players");
        AppSettings.Save("CHOSEN_GAME", game);
    }

    public void SelectRandomGame(int numberOfPlayers)
    {
        List<Constants.GameName> availables = (List<Constants.GameName>)AppSettings.Get(Constants.APPSETTINGS_PLAYABLEGAMES_LABEL);
        if (availables.Count == 0)
        {
            List<Constants.GameName> games = FilterGamesByPlayersNumber(numberOfPlayers);
            games.Shuffle();
            AppSettings.Save(Constants.APPSETTINGS_PLAYABLEGAMES_LABEL, games);
            availables = games;
        }
        SelectGame(availables[0], numberOfPlayers);
        availables.RemoveAt(0);
        AppSettings.Save(Constants.APPSETTINGS_PLAYABLEGAMES_LABEL, availables);
        this.gameObject.GetComponent<IGameLoader>().LoadGame();
    }

    private void Awake()
    {
        int players = (int)AppSettings.Get("N_PLAYERS");
        if (AppSettings.Get(Constants.APPSETTINGS_PLAYABLEGAMES_LABEL) == null)
        {
            if (players < 2 || players > 8) throw new System.ArgumentOutOfRangeException("Number of players not allowed. Valid range: 2 to 8.");
            List<Constants.GameName> games = FilterGamesByPlayersNumber(players);
            games.Shuffle();
            AppSettings.Save(Constants.APPSETTINGS_PLAYABLEGAMES_LABEL, games);
            Debug.Log($"Loaded {games.Count} playable games");
        }
        SelectRandomGame(players);
    }

}
