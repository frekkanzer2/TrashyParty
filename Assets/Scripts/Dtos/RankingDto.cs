using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingDto
{
    public class Rank
    {
        public int PlayerId;
        public Sprite PlayerSprite;
        public int Points;
        public Rank(int playerId, Sprite playerSprite)
        {
            this.PlayerId = playerId;
            this.PlayerSprite = playerSprite;
            this.Points = 0;
        }
        private Rank(int playerId, Sprite playerSprite, int points)
        {
            this.PlayerId = playerId;
            this.PlayerSprite = playerSprite;
            this.Points = points;
        }
        public Rank Clone() => new(this.PlayerId, this.PlayerSprite, this.Points);
    }

    private readonly List<Rank> ranks;

    private RankingDto(List<Rank> r) { ranks = r; }

    public static RankingDto Generate(List<Rank> ranks) => new(ranks);

    public void AddPoint(int playerId) => this.ranks.Find(r => r.PlayerId == playerId).Points++;

    public void AddPoint(int[] playerIds)
    {
        foreach(int id in playerIds)
            this.ranks.Find(r => r.PlayerId == id).Points++;
    }

    public List<Rank> GetRanking() {
        List<Rank> cloned = new();
        foreach (Rank r in this.ranks) cloned.Add(r.Clone());
        cloned.Sort((r1, r2) => r1.Points.CompareTo(r2.Points));
        return cloned;
    }

    public RankingDto Clone() => Generate(GetRanking());

}
