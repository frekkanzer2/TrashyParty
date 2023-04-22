using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void OnPreparationEnds();
    void SpawnPlayers();
    void ActiveGameObjectsBasedOnPlayerNumber();
    void GenerateTeams();
    void OnGameEnds();
    bool IsGameEnded();
    bool IsGameStarted();
    void AssignPoints();
}
