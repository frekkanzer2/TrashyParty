using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamDto
{
    public int Id;
    public List<IPlayer> players;
    public List<Transform> spawnpositions;
    public void KillRandomPlayer()
    {
        if (IsEveryoneDead()) return;
        List<IPlayer> alives = players.FindAll(p => p.IsAlive());
        alives[(new System.Random()).Next(alives.Count)].OnDeath();
    }
    public void KillAllPlayers()
    {
        if (IsEveryoneDead()) return;
        foreach (IPlayer p in players) p.OnDeath();
    }
    public bool IsEveryoneDead() => GetAlivePlayers().Count == 0;
    public List<IPlayer> GetAlivePlayers() => players.FindAll(p => p.IsAlive());
}
