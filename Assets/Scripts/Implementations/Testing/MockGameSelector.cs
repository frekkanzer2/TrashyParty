using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MockGameSelector : MonoBehaviour, IGameSelector
{

    [SerializeField] private int players;
    [SerializeField] private Constants.GameName gameName;

    public void SelectGame(Constants.GameName game)
    {
        AppSettings.Save("CHOSEN_GAME", game);
    }

    public void SelectRandomGame()
    {
        Constants.GameName game = (Constants.GameName)Random.Range(0, (int)System.Enum.GetValues(typeof(Constants.GameName)).Cast<Constants.GameName>().Max());
        SelectGame(game);
    }

    public void SetNumberOfPlayers(int number)
    {
        AppSettings.Save("N_PLAYERS", number);
    }

    private void Awake()
    {
        if (players < 2 || players > 8) throw new System.ArgumentOutOfRangeException("Number of players not allowed. Valid range: 2 to 8.");
        SetNumberOfPlayers(players);
        SelectGame(gameName);
    }

}
