using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamDto
{
    public int Id;
    public List<IPlayer> players;
    public List<Transform> spawnpositions;
    public void KillRandomPlayer() {
        if (AreEveryoneDead()) return;
        List<IPlayer> alives = players.FindAll(p => p.IsAlive());
        alives[(new System.Random()).Next(alives.Count)].OnDeath();
    }
    public bool AreEveryoneDead() => (players.FindAll(p => p.IsAlive()).Count > 0) ? false : true;
    public List<IPlayer> GetAlivePlayers() => players.FindAll(p => p.IsAlive());
}
