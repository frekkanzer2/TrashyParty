using System.Collections.Generic;

public interface IGameSelector{
    public void SelectRandomGame(int numberOfPlayers);
    public void SelectGame(Constants.GameName game, int numberOfPlayers);
    public List<Constants.GameName> FilterGamesByPlayersNumber(int numberOfPlayers);
    public bool IsGamePossibleByPlayersNumber(Constants.GameName name, int numberOfPlayers);
}
