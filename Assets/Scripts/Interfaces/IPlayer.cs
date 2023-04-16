public interface IPlayer
{
    bool IsInitialized { get; }
    void Initialize();
    void OnGameStarts();
    void OnGameEnds();
    void OnDeath();
    void OnSpawn();
    void SetGamepad(IGamepad gamepad);
    void SetGamepadByAssociation(PlayerControllerAssociationDto pcaDto);
}
