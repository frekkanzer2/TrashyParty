using UnityEngine;

public interface IPlayer
{
    bool IsInitialized { get; }
    void Initialize();
    void OnGameStarts();
    void OnGameEnds();
    void OnDeath();
    void OnSpawn();
    void SetAsReady();
    void SetAsNotReady();
    bool IsDead();
    bool IsAlive() => !IsDead();
    void SetGamepad(IGamepad gamepad);
    void SetGamepadByAssociation(PlayerControllerAssociationDto pcaDto);
    Sprite GetBirdSprite();
    void IgnoreCollisionsWithOtherPlayers(bool active);
    void ApplyForce(Vector2 force);
    void ApplyForce(Vector2 force, float countdownInSeconds);
    void SetCanWalk(bool b);
    string GetName();
    bool CheckName(string name);
    Vector3 RespawnPosition { get; set; }
}
