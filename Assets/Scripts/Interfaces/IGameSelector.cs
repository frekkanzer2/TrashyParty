public interface IGameSelector{
    public void SelectRandomGame(int numberOfPlayers);
    public void SelectGame(Constants.GameName game, int numberOfPlayers);
}
