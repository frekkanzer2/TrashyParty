public interface IGameSelector{
    public void SetNumberOfPlayers(int number);
    public void SelectRandomGame();
    public void SelectGame(Constants.GameName game);
}
