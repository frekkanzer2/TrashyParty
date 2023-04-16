using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void StartPreparationAnimation();
    void SpawnPlayers();
    void ActiveGameObjectsBasedOnPlayerNumber();
}
