using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MockGameSelector : MonoBehaviour, IGameSelector
{

    [SerializeField] private Constants.GameName gameName;

    public void SelectGame(Constants.GameName game, int numberOfPlayers)
    {
        int[] supportedPlayers;
        switch (game)
        {
            case Constants.GameName.BeachVolley:
                supportedPlayers = new int[] { 2, 3, 4, 6, 8 };
                break;
            case Constants.GameName.RocketBirdLeague:
                supportedPlayers = new int[] { 4, 6, 8 };
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
            default:
                throw new System.NullReferenceException("Missing game inside the game selection");
        }
        if (!supportedPlayers.Contains(numberOfPlayers)) throw new System.ArgumentException($"The game {game} doesn't supports {numberOfPlayers} players");
        AppSettings.Save("CHOSEN_GAME", game);
    }

    public void SelectRandomGame(int numberOfPlayers)
    {
        Constants.GameName game = (Constants.GameName)Random.Range(0, (int)System.Enum.GetValues(typeof(Constants.GameName)).Cast<Constants.GameName>().Max());
        SelectGame(game, numberOfPlayers);
    }

    private void Awake()
    {
        int players = (int)AppSettings.Get("N_PLAYERS");
        if (players < 2 || players > 8) throw new System.ArgumentOutOfRangeException("Number of players not allowed. Valid range: 2 to 8.");
        SelectGame(gameName, players);
    }

}
