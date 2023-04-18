using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void OnPreparationEnds();
    void SpawnPlayers();
    void ActiveGameObjectsBasedOnPlayerNumber();
    void GenerateTeams();
}
